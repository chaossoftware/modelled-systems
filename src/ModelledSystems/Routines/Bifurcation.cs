using ChaosSoft.Core.Data;
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
    private readonly SysParamCfg _param;
    private readonly double _step;

    public Bifurcation(string outDir, SystemCfg sysConfig, int paramIndex, int iterations) : base(outDir, sysConfig)
    {
        _solutionSeries = new DataSeries();
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

        _solutionSeries.DataPoints.AddRange(_dataPoints);

        DataWriter.CreateDataFile(Path.Combine(OutDir, SysConfig.Name + "_data_bifur_" + _param.Name), _solutionSeries.ToString());

        var plt = GetPlot(_param.Name, "x");
        plt.AddScatterPoints(_solutionSeries.XValues, _solutionSeries.YValues, Color.Blue, 1);
        plt.SaveFig(Path.Combine(OutDir, SysConfig.Name + "_bifur_" + _param.Name + ".png"));
    }


    private void Func(double p)
    {
        double[] vars = new double[SysConfig.ParamsValues.Length];
        Array.Copy(SysConfig.ParamsValues, vars, vars.Length);
        vars[_paramIndex] = p;

        SystemBase eq = GetSystemEquations(vars);

        var solver = GetSolver(eq);

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
