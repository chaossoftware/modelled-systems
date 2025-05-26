using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Ode;
using ChaosSoft.NumericalMethods.Lyapunov;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ScottPlot;
using ChaosSoft.NumericalMethods.Algebra;
using ModelledSystems.Configuration;

namespace ModelledSystems.Routines;

internal sealed class LleParam : Routine
{
    private readonly TaskProgress _progress;
    private readonly DataSeries _lleSeries;
    private readonly int _totalIterations;
    private readonly int _drivingParamIndex;
    private readonly double _paramStep;
    private readonly ConcurrentBag<DataPoint> _dataPoints;

    private readonly SysParamCfg _param;
    private readonly SolverType _solverType;
    private readonly long _iterations;
    private readonly double _dt;

    public LleParam(string outDir, Config config, int paramIndex, int iterations) 
        : base(outDir, config.System, config.Solver)
    {
        _lleSeries = new DataSeries();
        _drivingParamIndex = paramIndex;
        _param = SysConfig.Params[_drivingParamIndex];
        _totalIterations = iterations;
        _paramStep = (_param.To - _param.From) / _totalIterations;
        _dataPoints = new ConcurrentBag<DataPoint>();
        _progress = new TaskProgress(_totalIterations);
        _solverType = config.Solver.Type;

        _dt = config.Solver.Dt;
        _iterations = (long)(config.Solver.ModellingTime / _dt);
    }

    public override void Run()
    {
        Parallel.For(0, _totalIterations, i =>
        {
            CalculateLLeForParam(_param.From + _paramStep * i);
        });

        _lleSeries.DataPoints.AddRange(_dataPoints.Where(dp => !Numbers.IsNanOrInfinity(dp.Y)));
        _lleSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

        FileUtils.CreateDataFile(FileNameBase + "_data_lle_" + _param.Name, _lleSeries.ToString());

        Plot llePlot = GetPlot(_param.Name, "LLE");
        llePlot.AddScatter(_lleSeries.XValues, _lleSeries.YValues, Color.Blue, markerSize: 0);
        SavePlot(llePlot, FileNameBase + $"_lle_{_param.Name}.png");
    }

    private void CalculateLLeForParam(double paramValue)
    {
        double[] vars = SysConfig.ParamsValues.ToArray();
        vars[_drivingParamIndex] = paramValue;

        IOdeSys equations = GetSystemEquations(vars);


        OdeSolverBase solver = SolverFactory.Get(_solverType, equations, _dt);
        solver.SetInitialConditions(0, SysConfig.InitialConditions);

        OdeSolverBase solverCopy = SolverFactory.Get(_solverType, equations, _dt);
        solverCopy.SetInitialConditions(0, SysConfig.InitialConditions);

        LleBenettin benettin = new(solver, solverCopy, _iterations);
        benettin.Calculate();

        _dataPoints.Add(new DataPoint(paramValue, benettin.Result));
        _progress.Iterate();
    }
}
