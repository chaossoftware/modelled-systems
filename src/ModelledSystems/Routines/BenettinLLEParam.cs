using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal class BenettinLLEParam : Routine
{
    private readonly TaskProgress _progress;
    private readonly DataSeries _lleSeries;
    private readonly int _paramIndex;
    private readonly int _totalIterations;
    private readonly ConcurrentBag<DataPoint> _dataPoints;
    private readonly Parameter _param;
    private readonly double _step;

    public BenettinLLEParam(string outDir, SystemParameters systemParameters, int paramIndex, int iterations) : base(outDir, systemParameters)
    {
        _lleSeries = new DataSeries();
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

        _lleSeries.DataPoints.AddRange(_dataPoints);
        _lleSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

        DataWriter.CreateDataFile(Path.Combine(OutDir, SysParameters.SystemName + "_data_lle_" + _param.Name), _lleSeries.ToString());

        var plt = GetPlot(_param.Name, "LLE");
        plt.AddSignalXY(_lleSeries.XValues, _lleSeries.YValues, Color.Blue);
        plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_lle_" + _param.Name + ".png"));
    }


    private void Func(double p)
    {
        double[] vars = new double[SysParameters.Defaults.Length];
        Array.Copy(SysParameters.Defaults, vars, vars.Length);
        vars[_paramIndex] = p;

        SystemBase equations = GetSystemEquations(vars);
        Type solverType = GetSolverType(SysParameters.Solver);
        double eqStep = SysParameters.Step;
        long totIter = (long)(SysParameters.ModellingTime / eqStep);

        LleBenettin benettin = new LleBenettin(equations, solverType, eqStep, totIter);
        benettin.Calculate();

        _dataPoints.Add(new DataPoint(p, benettin.Result));

        _progress.Iterate();
    }

}
