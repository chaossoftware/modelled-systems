using ChaosSoft.Core;
using ChaosSoft.Core.DataUtils;
using ChaosSoft.Core.Logging;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.Ode.Linearized;
using ChaosSoft.NumericalMethods.QrDecomposition;

namespace ModelledSystems.Routines;

internal sealed class LeSpec : Routine
{
    private readonly long _iterations;
    private readonly int _eqCount;

    private readonly LeSpecBenettin _leSpec;
    private double[] _rMatrix;          //normalized vector (triangular matrix)
    //private readonly double[,] _leSpecInTime;

    private readonly ILinearizedOdeSys _equations;
    private readonly LinearizedOdeSolverBase _solver;
    private readonly double _dt;
    private readonly IQrDecomposition _orthogonalization;
    private readonly int _irate;

    private int j, i;            //counters 

    public LeSpec(string outDir, SystemCfg sysConfig, OrthogonalizationCfg orthogonalization) : 
        base (outDir, sysConfig)
    {
        _irate = orthogonalization.Interval;
        _dt = SysConfig.Solver.Dt;

        _equations = GetLinearizedSystemEquations(SysConfig.ParamsValues);
        _solver = GetLinearizedSolver(_equations);

        _eqCount = _equations.EqCount;

        _orthogonalization = GetOrthogonalization(orthogonalization.Type, _eqCount);
        _iterations = (long)(SysConfig.Solver.ModellingTime / _dt);
        _leSpec = new LeSpecBenettin(_eqCount);
        _rMatrix = new double[_eqCount];
        //_leSpecInTime = new double[_iterations, _eqCount];
    }

    public override void Run()
    {
        _solver.SetInitialConditions(0, GetInitialConditions());
        _solver.SetLinearInitialConditions(SysConfig.LinearInitialConditions);

        for (i = 0; i < _iterations; i++)
        {
            for (j = 0; j < _irate; j++)
            {
                _solver.NextStep();
            }

            if (_solver.IsSolutionDecayed())
            {
                Vector.FillWith(_leSpec.Result, double.NaN);
                break;
            }

            _rMatrix = _orthogonalization.Perform(_solver.Linearization);
            _leSpec.CalculateLyapunovSpectrum(_rMatrix, _solver.T);

            //------------------- normalize and print exponent ------------
            //for (j = 0; j < _eqCount; j++)
            //{
            //    _leSpecInTime[i, j] = _leSpec.Result[j];
            //}
        }

        WriteResults();
    }


    private void WriteResults()
    {
        Log.Info("LEs = {0}", NumFormat.Format(_leSpec.Result, Constants.LeNumFormat, " , "));
        Log.Info("Dky = {0}", NumFormat.Format(StochasticProperties.KYDimension(_leSpec.Result), Constants.LeNumFormat));
        Log.Info("Eks = {0}", NumFormat.Format(StochasticProperties.KSEntropy(_leSpec.Result), Constants.LeNumFormat));
        Log.Info("PVC = {0}", NumFormat.Format(StochasticProperties.PhaseVolumeContractionSpeed(_leSpec.Result), Constants.LeNumFormat));

        //FileUtils.CreateDataFile(FileNameBase + ".le", _leSpec.Result.ToString());

        //double t = 0;
        //StringBuilder output = new StringBuilder();

        //for (i = 0; i < _iterations; i++)
        //{
        //    output.AppendFormat("{0:F5}", t);

        //    for (j = 0; j < _eqCount; j++)
        //    {
        //        output.AppendFormat("\t{0:F15}", _leSpecInTime[i, j]);
        //    }

        //    output.AppendLine();

        //    t += _dt;
        //}

        //DataWriter.CreateDataFile(FileNameBase + "_inTime.le", output.ToString());
    }
}
