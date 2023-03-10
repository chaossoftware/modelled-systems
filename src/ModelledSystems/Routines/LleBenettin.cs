using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using System;
using System.IO;

namespace ModelledSystems.Routines;

class LleBenettin : Routine
{
    private readonly double _eqStep;
    private string fileNameStart;
    private readonly SystemBase _equations;
    private readonly Type _solverType;

    //double[] outArray;

    private long TotIter;

    public LleBenettin(string outDir, SystemCfg sysConfig) : base (outDir, sysConfig)
    {
        _equations = GetSystemEquations(SysConfig.ParamsValues);
        _solverType = GetSolverType();
        _eqStep = SysConfig.Solver.Dt;
        TotIter = (long)(SysConfig.Solver.ModellingTime / _eqStep) * 1000;
        fileNameStart = Path.Combine(OutDir, _equations.Name);
        //outArray = new double[TotIter];
    }

    public override void Run()
    {
        ChaosSoft.NumericalMethods.Lyapunov.LleBenettin benettin = new ChaosSoft.NumericalMethods.Lyapunov.LleBenettin(_equations, _solverType, _eqStep, TotIter);
        benettin.Calculate();
        //WriteResults();
        Console.WriteLine(benettin.ToString());
        Console.WriteLine("\nLLE = " + benettin.GetResultAsString());
    }

    private void WriteResults()
    {
        //Console.WriteLine("{0:F5}", l1);
        //DataWriter.CreateDataFile(equations.SystemName + "_inTime.le", output.ToString());
        //StringBuilder output = new StringBuilder();
        
        /*
        double t = 0;
        for (int cnt = 0; cnt < TotIter; cnt++)
        {
            output.AppendFormat("{0:F5}\t{0:F15}\n", t, outArray[cnt]);
            t += EqStep;
        }
        DataWriter.CreateDataFile(eq.SystemName + "_inTime.le", output.ToString());
        */
    }
}
