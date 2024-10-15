using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Ode;
using ScottPlot;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal sealed class Bifurcation : Routine
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

        float ptSize = _totalIterations switch
        {
            < 1000 => 0.5f,
            < 2500 => 0.25f,
            _ => 0.13f
        };

        _solutions.DataPoints.AddRange(_dataPoints);

        FileUtils.CreateDataFile(FileNameBase + "_data_bifur_" + _param.Name, _solutions.ToString());

        Plot bifurcationPlot = GetPlot(_param.Name, "x");
        bifurcationPlot.AddScatterPoints(_solutions.XValues, _solutions.YValues, Color.Blue, ptSize);

        SavePlot(bifurcationPlot, FileNameBase + $"_bifur_{_param.Name}.png");
    }

    private void SolveEquationsFor(double paramValue)
    {
        double[] vars = SysConfig.ParamsValues.ToArray();
        vars[_drivingParamIndex] = paramValue;

        IOdeSys eq = GetSystemEquations(vars);
        OdeSolverBase solver = GetSolver(eq);
        solver.SetInitialConditions(0, GetInitialConditions());

        for (int j = 0; j < _totalIterations; j++)
        {
            solver.NextStep();

            if (j > _totalIterations - _lastIter)
            {
                double rez = solver.Solution[0];

                if (!solver.IsSolutionDecayed() && Math.Abs(rez) < 1000)
                {
                    _dataPoints.Add(new DataPoint(paramValue, rez));
                }
            }
        }

        _progress.Iterate();
    }
}
