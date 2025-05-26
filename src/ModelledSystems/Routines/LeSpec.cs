using ChaosSoft.Core;
using ChaosSoft.Core.Logging;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.Ode.Linearized;
using ChaosSoft.NumericalMethods.QrDecomposition;
using ModelledSystems.Configuration;

namespace ModelledSystems.Routines;

internal sealed class LeSpec : Routine
{
    private readonly long _iterations;
    private readonly int _eqCount;

    private LeSpecBenettin leSpec;

    private readonly ILinearizedOdeSys _equations;
    private readonly LinearizedOdeSolverBase _solver;
    private readonly double _dt;
    private readonly IQrDecomposition _orthogonalization;
    private readonly int _irate;

    public LeSpec(string outDir, Config config) : 
        base(outDir, config.System, config.Solver)
    {
        _irate = config.Task.Orthogonalization.Interval;
        _dt = config.Solver.Dt;

        _equations = GetLinearizedSystemEquations(SysConfig.ParamsValues);
        _solver = GetLinearizedSolver(_equations);

        _eqCount = _equations.EqCount;

        _orthogonalization = GetOrthogonalization(config.Task.Orthogonalization.Type, _eqCount);
        _iterations = (long)(config.Solver.ModellingTime / _dt);
    }

    public override void Run()
    {
        _solver.SetInitialConditions(0, SysConfig.InitialConditions);
        _solver.SetLinearInitialConditions(SysConfig.LinearInitialConditions);

        leSpec = new LeSpecBenettin(_solver, _iterations, _orthogonalization, _irate);
        leSpec.Calculate();

        Log.Info(leSpec.ToString());

        Log.Info("LEs = {0}", NumFormat.Format(leSpec.Result, Constants.LeNumFormat, " "));
        Log.Info("Dky = {0}", NumFormat.Format(StochasticProperties.KYDimension(leSpec.Result), Constants.LeNumFormat));
        Log.Info("Eks = {0}", NumFormat.Format(StochasticProperties.KSEntropy(leSpec.Result), Constants.LeNumFormat));
        Log.Info("PVC = {0}", NumFormat.Format(StochasticProperties.PhaseVolumeContractionSpeed(leSpec.Result), Constants.LeNumFormat));
    }
}
