using ChaosSoft.NumericalMethods.Ode;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.Core.Logging;
using ChaosSoft.Core;

namespace ModelledSystems.Routines;

internal sealed class Lle : Routine
{
    private readonly double _eqStep;
    private readonly IOdeSys _equations;
    private readonly SolverType _solverType;
    private readonly long _totalIterations;

    //double[] outArray;


    public Lle(string outDir, SystemCfg sysConfig) : base (outDir, sysConfig)
    {
        _equations = GetSystemEquations(SysConfig.ParamsValues);
        _solverType = SysConfig.Solver.Type;
        _eqStep = SysConfig.Solver.Dt;
        _totalIterations = (long)(SysConfig.Solver.ModellingTime / _eqStep);
        //outArray = new double[TotIter];
    }

    public override void Run()
    {
        LleBenettin benettin = new(_equations, _solverType, GetInitialConditions(), _eqStep, _totalIterations);
        benettin.Calculate();
        //WriteResults();
        Log.Info(benettin.ToString());
        Log.Info("\nLLE = {0}", NumFormat.Format(benettin.Result, Constants.LeNumFormat));
    }

    private void WriteResults()
    {
        //Console.WriteLine("{0:F5}", l1);
        //DataWriter.CreateDataFile(FileNameBase + "_inTime.le", output.ToString());
        //StringBuilder output = new StringBuilder();

        /*
        double t = 0;
        for (int cnt = 0; cnt < TotIter; cnt++)
        {
            output.AppendFormat("{0:F5}\t{0:F15}\n", t, outArray[cnt]);
            t += EqStep;
        }
        DataWriter.CreateDataFile(FileNameBase + "_inTime.le", output.ToString());
        */
    }
}
