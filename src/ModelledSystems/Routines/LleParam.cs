using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ModelledSystems.Routines;

internal class LleParam : Routine
{
    private readonly TaskProgress _progress;
    private readonly DataSeries _lleSeries;
    private readonly int _totalIterations;
    private readonly int _drivingParamIndex;
    private readonly double _paramStep;
    private readonly ConcurrentBag<DataPoint> _dataPoints;

    private readonly SysParamCfg _param;
    private readonly Type _solverType;
    private readonly long _iterations;
    private readonly double _dt;

    public LleParam(string outDir, SystemCfg sysConfig, int paramIndex, int iterations) 
        : base(outDir, sysConfig)
    {
        _lleSeries = new DataSeries();
        _drivingParamIndex = paramIndex;
        _param = sysConfig.Params[_drivingParamIndex];
        _totalIterations = iterations;
        _paramStep = (_param.To - _param.From) / _totalIterations;
        _dataPoints = new ConcurrentBag<DataPoint>();
        _progress = new TaskProgress(_totalIterations);
        _solverType = GetSolverType();

        _dt = sysConfig.Solver.Dt;
        _iterations = (long)(sysConfig.Solver.ModellingTime / _dt);
    }

    public override void Run()
    {
        Parallel.For(0, _totalIterations, i =>
        {
            CalculateLLeForParam(_param.From + _paramStep * i);
        });

        _lleSeries.DataPoints.AddRange(_dataPoints);
        _lleSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

        DataWriter.CreateDataFile(FileNameBase + "_data_lle_" + _param.Name, _lleSeries.ToString());

        var plt = GetPlot(_param.Name, "LLE");
        plt.AddSignalXY(_lleSeries.XValues, _lleSeries.YValues, Color.Blue);
        plt.SaveFig(FileNameBase + $"_lle_{_param.Name}.png");
    }

    private void CalculateLLeForParam(double paramValue)
    {
        double[] vars = SysConfig.ParamsValues.ToArray();
        vars[_drivingParamIndex] = paramValue;

        SystemBase equations = GetSystemEquations(vars);
        LleBenettin benettin = new LleBenettin(equations, _solverType, _dt, _iterations);
        benettin.Calculate();

        _dataPoints.Add(new DataPoint(paramValue, benettin.Result));
        _progress.Iterate();
    }
}
