using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using System;

namespace ModelledSystems.Routines;

class Lle : Routine
{
    private readonly double _eqStep;
    private readonly SystemBase _equations;
    private readonly Type _solverType;

    //double[] outArray;

    private long TotIter;

    public Lle(string outDir, SystemCfg sysConfig) : base (outDir, sysConfig)
    {
        _equations = GetSystemEquations(SysConfig.ParamsValues);
        _solverType = GetSolverType();
        _eqStep = SysConfig.Solver.Dt;
        TotIter = (long)(SysConfig.Solver.ModellingTime / _eqStep);
        //outArray = new double[TotIter];
    }

    public override void Run()
    {
        LleBenettin benettin = new LleBenettin(_equations, _solverType, _eqStep, TotIter);
        benettin.Calculate();
        //WriteResults();
        Console.WriteLine(benettin.ToString());
        Console.WriteLine("\nLLE = " + benettin.GetResultAsString());
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
