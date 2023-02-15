﻿using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal class Bifurcation : Routine
{
    private readonly TaskProgress _progress;
    private readonly ConcurrentBag<DataPoint> _dataPoints;
    private readonly int _totalIterations;
    private readonly DataSeries _solutionSeries;
    private readonly int _paramIndex;
    private readonly int _lastIter = 100;
    private readonly Parameter _param;
    private readonly double _step;

    public Bifurcation(string outDir, SystemParameters systemParameters, int paramIndex, int iterations) : base(outDir, systemParameters)
    {
        _solutionSeries = new DataSeries();
        _paramIndex = paramIndex;
        _param = SysParameters.ListParameters[_paramIndex];
        _totalIterations = iterations;
        _step = (_param.End - _param.Start) / _totalIterations;
        _dataPoints = new ConcurrentBag<DataPoint>();
        _progress = new TaskProgress(_totalIterations);
    }

    public override void Run()
    {
        Parallel.For(0, _totalIterations, i =>
        {
            Func(_param.Start + _step * i);
        });

        _solutionSeries.DataPoints.AddRange(_dataPoints);

        DataWriter.CreateDataFile(Path.Combine(OutDir, SysParameters.SystemName + "_dataBifur_" + _param.Name), _solutionSeries.ToString());

        var plt = new ScottPlot.Plot(Size.Width, Size.Height);
        plt.XAxis.Label(_param.Name);
        plt.YAxis.Label("X");

        foreach (DataPoint dp in _solutionSeries.DataPoints)
        {
            plt.AddPoint(dp.X, dp.Y, Color.Blue, 1);
        }

        plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_bifur_" + _param.Name + ".png"));
    }


    private void Func(double p)
    {
        double[] vars;

        vars = SysParameters.Defaults;
        vars[_paramIndex] = p;

        SystemBase eq = GetSystemEquations(vars);

        var solver = GetSolver(SysParameters.Solver, eq, SysParameters.Step);

        for (int j = 0; j < _totalIterations; j++)
        {
            solver.NexStep();

            if (j > _totalIterations - _lastIter)
            {
                double rez = solver.Solution[0, 0];

                if (!double.IsInfinity(rez) && !double.IsNaN(rez) && Math.Abs(rez) < 1000)
                {
                    _dataPoints.Add(new DataPoint(p, rez));
                }
            }
        }

        _progress.Iterate();
    }
}
