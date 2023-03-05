using ChaosSoft.Core.Data;
using ModelledSystems.Equations.Augmented;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal class SynchronizationLLE : Routine
{
    private readonly TaskProgress _progress;

    private readonly DataSeries _syncSeries;
    private readonly int _pIter;
    private readonly double _pStep;
    private readonly int _totalIterations;
    private readonly int _lastIter = 100;
    private readonly ConcurrentBag<DataPoint> _dataPoints;

    public SynchronizationLLE(string outDir, SystemParameters systemParameters, int pIter, double pstep) : base (outDir, systemParameters)
    {
        _syncSeries = new DataSeries();
        _pIter = pIter;
        _pStep = pstep;
        _totalIterations = (int)(SysParameters.ModellingTime * SysParameters.Step);
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
        plt.AddScatterPoints(_syncSeries.XValues, _syncSeries.YValues, Color.Blue, 1)

        plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_stefanski.png"));

        int k = _syncSeries.Length - 1;
        double rezY = _syncSeries.DataPoints[k].Y;
        bool sync = true;

        while (sync)
        {
            sync = Math.Abs(_syncSeries.DataPoints[--k].Y - rezY) < 1e-8;
        }

        Console.WriteLine(_syncSeries.DataPoints[k++].X.ToString("F5"));
    }


    private void Func(double p)
    {
        AugmentedEquations augmentedEquations = GetSystemEquations();
        augmentedEquations.p = p;

        var solver = GetSolver(SysParameters.Solver, augmentedEquations, SysParameters.Step);

        for (int j = 0; j < _totalIterations; j++)
        {
            solver.NexStep();
            if (j > _totalIterations - _lastIter)
            {
                double rez = solver.Solution[0, augmentedEquations.Count - augmentedEquations.Count / 3];
                if (!double.IsInfinity(rez) && !double.IsNaN(rez) && rez < 100 && rez > -100)
                    _dataPoints.Add(new DataPoint(p, rez));
            }
        }

        _progress.Iterate();
    }


    private AugmentedEquations GetSystemEquations()
    {
        switch (SysParameters.SystemName.ToLower())
        {
            case "lorenz":
                return new LorenzAugmented();
            case "rossler":
                return new RosslerAugmented();
            case "henon":
                return new HenonAugmented();
            case "henon_generalized":
                return new HenonGeneralizedAugmented();
            case "logistic":
                return new LogisticAugmented();
            case "tinkerbell":
                return new TinkerbellAugmented();
            default:
                throw new ArgumentException();
        }
    }

}
