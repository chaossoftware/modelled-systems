using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal class Bifurcation : Routine
{
    private readonly TaskProgress _progress;
    private readonly ConcurrentBag<DataPoint> _dataPoints;
    private readonly int _totalIterations;
    private readonly DataSeries _solutions;
    private readonly int _drivingParamIndex;
    private readonly int _lastIter = 100;
    private readonly SysParamCfg _param;
    private readonly double _paramStep;

    public Bifurcation(string outDir, SystemCfg sysConfig, int paramIndex, int iterations) 
        : base(outDir, sysConfig)
    {
        _solutions = new DataSeries();
        _drivingParamIndex = paramIndex;
        _param = sysConfig.Params[_drivingParamIndex];
        _totalIterations = iterations;
        _paramStep = (_param.To - _param.From) / _totalIterations;
        _dataPoints = new ConcurrentBag<DataPoint>();
        _progress = new TaskProgress(_totalIterations);
    }

    public override void Run()
    {
        Parallel.For(0, _totalIterations, i =>
        {
            SolveEquationsFor(_param.From + _paramStep * i);
        });

        _solutions.DataPoints.AddRange(_dataPoints);

        DataWriter.CreateDataFile(FileNameBase + "_data_bifur_" + _param.Name, _solutions.ToString());

        var plt = GetPlot(_param.Name, "x");
        plt.AddScatterPoints(_solutions.XValues, _solutions.YValues, Color.Blue, 1);
        plt.SaveFig(FileNameBase + $"_bifur_{_param.Name}.png");
    }

    private void SolveEquationsFor(double paramValue)
    {
        double[] vars = SysConfig.ParamsValues.ToArray();
        vars[_drivingParamIndex] = paramValue;

        SystemBase eq = GetSystemEquations(vars);
        SolverBase solver = GetSolver(eq);

        for (int j = 0; j < _totalIterations; j++)
        {
            solver.NexStep();

            if (j > _totalIterations - _lastIter)
            {
                double rez = solver.Solution[0, 0];

                if (!double.IsInfinity(rez) && !double.IsNaN(rez) && Math.Abs(rez) < 1000)
                {
                    _dataPoints.Add(new DataPoint(paramValue, rez));
                }
            }
        }

        _progress.Iterate();
    }
}
