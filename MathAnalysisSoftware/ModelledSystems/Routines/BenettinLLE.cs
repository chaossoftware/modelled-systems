using MathLib.MathMethods.Solvers;
using System;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines
{
    class BenettinLLE : Routine
    {
        //double[] outArray;

        double l1;

        long TotIter;
        int EqN;
        double EqStep;

        SystemEquations eq, eq1;

        double lsum;
        long nl;

        public BenettinLLE(string outDir, SystemParameters systemParameters) : base (outDir, systemParameters)
        {
            EqStep = SysParameters.Step.Default;

            eq = GetSystemEquations(false, SysParameters.Defaults, EqStep);
            eq1 = GetSystemEquations(false, SysParameters.Defaults, EqStep);

            EqN = eq.N;

            TotIter = (long)(SysParameters.ModellingTime / EqStep);
            //outArray = new double[TotIter];
            eq.Solver.Init();
        }

        public override void Run()
        {
            for (int i = 0; i < TotIter; i++)
            {
                MakeIteration();
                //outArray[i] = l1;
            }
                

            WriteResults();
        }


        private void MakeIteration()
        {
            eq.Solver.NexStep();

            if (eq1.Solver.Solution[0, 0] == 0)
            {
                eq1.Solver.Solution[0, 0] += eq.Solver.Solution[0, 0] + 1e-8;
                for (int _i = 1; _i < eq.N; _i++)
                    eq1.Solver.Solution[0, _i] = eq.Solver.Solution[0, _i];
                lsum = 0;
                nl = 0;
                return;
            }

            eq1.Solver.NexStep();

            double dl2 = 0;
            for (int _i = 0; _i < eq.N; _i++)
                dl2 += Math.Pow(eq1.Solver.Solution[0, _i] - eq.Solver.Solution[0, _i], 2);

            if (dl2 > 0)
            {
                double df = 1e16 * dl2;
                double rs = 1 / Math.Sqrt(df);

                for (int _i = 0; _i < eq.N; _i++)
                    eq1.Solver.Solution[0, _i] = eq.Solver.Solution[0, _i] + rs * (eq1.Solver.Solution[0, _i] - eq.Solver.Solution[0, _i]);
                lsum += Math.Log(df);
                nl++;
            }

            l1 = 0.5 * lsum / nl / Math.Abs(eq.Solver.Step);
        }


        private void WriteResults()
        {
            string fileNameStart = Path.Combine(OutDir, eq.SystemName);

            Console.WriteLine("{0:F5}", l1);
            //DataWriter.CreateDataFile(equations.SystemName + "_inTime.le", output.ToString());
            StringBuilder output = new StringBuilder();
            
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
}
