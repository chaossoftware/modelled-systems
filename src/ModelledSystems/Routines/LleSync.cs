using ChaosSoft.Core;
using ChaosSoft.Core.Data;
using ChaosSoft.Core.DataUtils;
using ChaosSoft.Core.Logging;
using ModelledSystems.Configuration;
using ModelledSystems.Equations.Augmented;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal sealed class LleSync : Routine
{
    private readonly TaskProgress _progress;

    private readonly DataSeries _syncSeries;
    private readonly int _pIter;
    private readonly double _pStep;
    private readonly int _totalIterations;
    private readonly int _lastIter = 100;
    private readonly ConcurrentBag<DataPoint> _dataPoints;

    public LleSync(string outDir, SystemCfg sysConfig, int pIter, double pstep) : base (outDir, sysConfig)
    {
        _syncSeries = new DataSeries();
        _pIter = pIter;
        _pStep = pstep;
        _totalIterations = (int)(SysConfig.Solver.ModellingTime * SysConfig.Solver.Dt);
        _dataPoints = new ConcurrentBag<DataPoint>();
        _progress = new TaskProgress(_totalIterations);
    }

    public override void Run()
    {
        Parallel.For(0, _pIter, i =>
        {
            Func(_pStep * i);
        });

        _syncSeries.DataPoints.AddRange(_dataPoints);

        _syncSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

        //DataWriter.CreateDataFile("fileName", SyncMapSeries.ToString());

        int k = _syncSeries.Length - 1;
        double rezY = _syncSeries.DataPoints[k].Y;
        bool sync = true;

        while (sync)
        {
            sync = Math.Abs(_syncSeries.DataPoints[--k].Y - rezY) < 1e-8;
        }

        double lle = _syncSeries.DataPoints[k++].X;
        string result = NumFormat.Format(lle, Constants.LeNumFormat);
        Log.Info("\nLLE = {0}", result);

        Plot leSyncPlot = GetPlot("p", "Δ");
        leSyncPlot.AddScatterPoints(_syncSeries.XValues, _syncSeries.YValues, Color.Blue, 0.25f);
        leSyncPlot.AddVerticalLine(lle, Color.IndianRed, 1, LineStyle.DashDot);
        leSyncPlot.SetAxisLimitsX(0, lle * 1.1);
        Annotation annotation = leSyncPlot.AddAnnotation(result, Alignment.UpperLeft);
        annotation.Shadow = false;

        SavePlot(leSyncPlot, FileNameBase + "_lle_sync.png");
    }

    private void Func(double p)
    {
        IAugmentedEquations augmentedEquations = GetSystemEquations();
        (augmentedEquations as IHasParameters).SetParameters(SysConfig.ParamsValues);
        augmentedEquations.P = p;

        var solver = GetSolver(augmentedEquations);
        solver.SetInitialConditions(0, GetInitialConditions(augmentedEquations.EqCount));

        for (int j = 0; j < _totalIterations; j++)
        {
            solver.NextStep();

            if (j > _totalIterations - _lastIter)
            {
                double rez = solver.Solution[augmentedEquations.EqCount - augmentedEquations.EqCount / 3];

                if (!double.IsInfinity(rez) && !double.IsNaN(rez) && rez < 100 && rez > -100)
                {
                    _dataPoints.Add(new DataPoint(p, rez));
                }
            }
        }

        _progress.Iterate();
    }

    private IAugmentedEquations GetSystemEquations()
    {
        return SysConfig.Name.ToLowerInvariant() switch
        {
            "lorenz" => new LorenzAugmented(),
            "rossler" => new RosslerAugmented(),
            "henon" => new HenonAugmented(),
            "henon_generalized" => new HenonGeneralizedAugmented(),
            "logistic" => new LogisticAugmented(),
            "tinkerbell" => new TinkerbellAugmented(),
            _ => throw new ArgumentException(),
        };
    }

    public static double[] GetInitialConditions(int eqCount)
    {
        double[] array = new double[eqCount];
        Vector.FillWith(array, 1e-8);
        return array;
    }
}
