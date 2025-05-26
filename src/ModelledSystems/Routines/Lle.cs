using ChaosSoft.NumericalMethods.Ode;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.Core.Logging;
using ChaosSoft.Core;
using ModelledSystems.Configuration;

namespace ModelledSystems.Routines;

internal sealed class Lle : Routine
{
    private readonly double _eqStep;
    private readonly IOdeSys _equations;
    private readonly SolverType _solverType;
    private readonly long _totalIterations;

    public Lle(string outDir, Config config) : base (outDir, config.System, config.Solver)
    {
        _eqStep = config.Solver.Dt;
        _equations = GetSystemEquations(SysConfig.ParamsValues);
        _solverType = config.Solver.Type;
        
        _totalIterations = (long)(config.Solver.ModellingTime / _eqStep);
    }

    public override void Run()
    {
        OdeSolverBase solver = SolverFactory.Get(_solverType, _equations, _eqStep);
        solver.SetInitialConditions(0, SysConfig.InitialConditions);

        OdeSolverBase solverCopy = SolverFactory.Get(_solverType, _equations, _eqStep);
        solverCopy.SetInitialConditions(0, SysConfig.InitialConditions);

        LleBenettin benettin = new(solver, solverCopy, _totalIterations);

        benettin.Calculate();
        Log.Info(benettin.ToString());
        Log.Info("\nLLE = {0}", NumFormat.Format(benettin.Result, Constants.LeNumFormat));
    }
}
