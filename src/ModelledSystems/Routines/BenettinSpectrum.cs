using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.Orthogonalization;
using System;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines;

internal class BenettinSpectrum : Routine
{
    private readonly long _iterations;
    private readonly int _eqCount;

    private readonly LeSpecBenettin _leSpec;
    private double[] _rMatrix;          //normalized vector (triangular matrix)
    //private readonly double[,] _leSpecInTime;

    private readonly SystemBase _equations;
    private readonly SolverBase _solver;
    private readonly double _dt;
    private readonly OrthogonalizationBase _orthogonalization;
    private readonly int _irate;

    private int j, i;            //counters 

    public BenettinSpectrum(string outDir, SystemCfg sysConfig, OrthogonalizationCfg orthogonalization) : 
        base (outDir, sysConfig)
    {
        _irate = orthogonalization.Interval;
        _dt = SysConfig.Solver.Dt;

        _equations = GetLinearizedSystemEquations(SysConfig.ParamsValues);
        _solver = GetSolver(_equations);

        _eqCount = _equations.Count;

        _orthogonalization = GetOrthogonalization(orthogonalization.Type, _eqCount);
        _iterations = (long)(SysConfig.Solver.ModellingTime / _dt);
        _leSpec = new LeSpecBenettin(_eqCount);
        _rMatrix = new double[_eqCount];
        //_leSpecInTime = new double[_iterations, _eqCount];
    }

    public override void Run()
    {
        for (i = 0; i < _iterations; i++)
        {
            for (j = 0; j < _irate; j++)
            {
                _solver.NexStep();
            }

            //------------------- Call Orthonormalization -------------
            _orthogonalization.Perform(_solver.Solution, _rMatrix);

            _leSpec.CalculateLyapunovSpectrum(_rMatrix, _solver.Time);

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
        Console.WriteLine("LES = " + Format.General(_leSpec.Result, ",", 5));

        Console.WriteLine($"Dky = {Format.General(StochasticProperties.KYDimension(_leSpec.Result), 5)}");
        Console.WriteLine($"Eks = {Format.General(StochasticProperties.KSEntropy(_leSpec.Result), 5)}");
        Console.WriteLine($"PVC = {Format.General(StochasticProperties.PhaseVolumeContractionSpeed(_leSpec.Result), 5)}");

        string fileNameStart = Path.Combine(OutDir, _equations.ToFileName());

        DataWriter.CreateDataFile(fileNameStart + ".le", _leSpec.Result.ToString());
        
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

        //DataWriter.CreateDataFile(SysConfig.Name + "_inTime.le", output.ToString());
    }
}
