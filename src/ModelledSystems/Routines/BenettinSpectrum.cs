using ChaosSoft.Core.IO;
using ChaosSoft.Core.NumericalMethods.Lyapunov;
using ChaosSoft.Core.NumericalMethods.Orthogonalization;
using ChaosSoft.Core.NumericalMethods.Solvers;
using System;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines
{
    class BenettinSpectrum : Routine
    {
        int i, counter;            //counters 
        long TotIter;
        int EqN;
        double EqStep;

        double[] R;          //normalized vector (triangular matrix)

        //double[,] outArray;

        static Orthogonalization ort;
        static BenettinMethod lyap;
        static SystemEquations Equations;
        
        string Orthogonalization;
        int Irate;

        public BenettinSpectrum(string outDir, SystemParameters systemParameters, string orthhogonalization, int irate) : base (outDir, systemParameters)
        {
            Orthogonalization = orthhogonalization;
            Irate = irate;
            EqStep = SysParameters.Step.Default;

            Equations = GetSystemEquations(true, SysParameters.Defaults, EqStep);
            EqN = Equations.EquationsCount;

            ort = GetOrthogonalization(Orthogonalization);
            TotIter = (long)(SysParameters.ModellingTime / EqStep);
            lyap = new BenettinMethod(EqN);
            R = new double[EqN];
            //outArray = new double[TotIter, EqN];
            Equations.Solver.Init();
        }

        public override void Run()
        {
            for (counter = 0; counter < TotIter; counter++)
            {
                for (i = 0; i < Irate; i++)
                    Equations.Solver.NexStep();

                //------------------- Call Orthonormalization -------------
                ort.Perform(Equations.Solver.Solution, R);

                lyap.CalculateLyapunovSpectrum(R, Equations.Solver.Time);

                //------------------- normalize and print exponent ------------
                //for (int k = 0; k < equations.N; k++)
                //  outArray[counter, k] = lyap.lespec[k];
            }

            WriteResults();
        }


        private void WriteResults()
        {
            Console.WriteLine(lyap.Result.ToString());
            string fileNameStart = Path.Combine(OutDir, Equations.ToFileName());

            DataWriter.CreateDataFile(fileNameStart + ".le", lyap.Result.ToString());

            
            StringBuilder output = new StringBuilder();
            /*
            double t = 0;
            for (int cnt = 0; cnt < TotIter; cnt++)
            {
                output.AppendFormat("{0:F5}", t);

                for (int k = 0; k < EqN; k++)
                    output.AppendFormat("\t{0:F15}", outArray[cnt, k]);

                output.Append("\n");

                t += EqStep;
            }
            DataWriter.CreateDataFile(equations.SystemName + "_inTime.le", output.ToString());
            */
        }


        private Orthogonalization GetOrthogonalization(string ortType)
        {
            ortType = ortType.ToLower();

            if (ortType == "mgs")
                return new ModifiedGrammSchmidt(EqN);
            else if (ortType == "cgs")
                return new ClassicGrammSchmidt(EqN);
            else if (ortType == "hh")
                return new HouseholderTransformation(EqN);

            return new ModifiedGrammSchmidt(EqN);
        }
    }
}
