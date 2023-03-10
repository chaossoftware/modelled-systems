using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ModelledSystems.Equations.Augmented;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal class LleSync : Routine
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

        var plt = GetPlot("p", "Δ");
        plt.AddScatterPoints(_syncSeries.XValues, _syncSeries.YValues, Color.Blue, 1);

        plt.SaveFig(Path.Combine(OutDir, SysConfig.Name + "_lyapunov_stefanski.png"));

        int k = _syncSeries.Length - 1;
        double rezY = _syncSeries.DataPoints[k].Y;
        bool sync = true;

        while (sync)
        {
            sync = Math.Abs(_syncSeries.DataPoints[--k].Y - rezY) < 1e-8;
        }

        Console.WriteLine("\nLLE = " + Format.General(_syncSeries.DataPoints[k++].X, 5));
    }

    private void Func(double p)
    {
        AugmentedEquations augmentedEquations = GetSystemEquations();
        augmentedEquations.p = p;

        var solver = GetSolver(augmentedEquations);

        for (int j = 0; j < _totalIterations; j++)
        {
            solver.NexStep();

            if (j > _totalIterations - _lastIter)
            {
                double rez = solver.Solution[0, augmentedEquations.Count - augmentedEquations.Count / 3];

                if (!double.IsInfinity(rez) && !double.IsNaN(rez) && rez < 100 && rez > -100)
                {
                    _dataPoints.Add(new DataPoint(p, rez));
                }
            }
        }

        _progress.Iterate();
    }

    private AugmentedEquations GetSystemEquations()
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

}
