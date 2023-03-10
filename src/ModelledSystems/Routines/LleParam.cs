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

internal class LleParam : Routine
{
    private readonly TaskProgress _progress;
    private readonly DataSeries _lleSeries;
    private readonly int _paramIndex;
    private readonly int _totalIterations;
    private readonly ConcurrentBag<DataPoint> _dataPoints;
    private readonly SysParamCfg _param;
    private readonly double _step;

    public LleParam(string outDir, SystemCfg sysConfig, int paramIndex, int iterations) : base(outDir, sysConfig)
    {
        _lleSeries = new DataSeries();
        _paramIndex = paramIndex;
        _param = SysConfig.Params[_paramIndex];
        _totalIterations = iterations;
        _step = (_param.To - _param.From) / _totalIterations;
        _dataPoints = new ConcurrentBag<DataPoint>();
        _progress = new TaskProgress(_totalIterations);
    }

    public override void Run()
    {
        Parallel.For(0, _totalIterations, i =>
        {
            Func(_param.From + _step * i);
        });

        _lleSeries.DataPoints.AddRange(_dataPoints);
        _lleSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

        DataWriter.CreateDataFile(Path.Combine(OutDir, SysConfig.Name + "_data_lle_" + _param.Name), _lleSeries.ToString());

        var plt = GetPlot(_param.Name, "LLE");
        plt.AddSignalXY(_lleSeries.XValues, _lleSeries.YValues, Color.Blue);
        plt.SaveFig(Path.Combine(OutDir, SysConfig.Name + "_lle_" + _param.Name + ".png"));
    }


    private void Func(double p)
    {
        double[] vars = new double[SysConfig.ParamsValues.Length];
        Array.Copy(SysConfig.ParamsValues, vars, vars.Length);
        vars[_paramIndex] = p;

        SystemBase equations = GetSystemEquations(vars);
        Type solverType = GetSolverType();
        double eqStep = SysConfig.Solver.Dt;
        long totIter = (long)(SysConfig.Solver.ModellingTime / eqStep);

        ChaosSoft.NumericalMethods.Lyapunov.LleBenettin benettin = new ChaosSoft.NumericalMethods.Lyapunov.LleBenettin(equations, solverType, eqStep, totIter);
        benettin.Calculate();

        _dataPoints.Add(new DataPoint(p, benettin.Result));

        _progress.Iterate();
    }

}
