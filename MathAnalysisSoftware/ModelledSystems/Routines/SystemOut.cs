using MathLib.IO;
using MathLib.MathMethods.Solvers;
using MathLib.Transform;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines
{
    class SystemOut : Routine
    {
        long TotIter;
        int EqN;
        double EqStep;
        double[,] outArray;
        SystemEquations Equations;

        public SystemOut(string outDir, SystemParameters systemParameters) : base (outDir, systemParameters)
        {
            EqStep = SysParameters.Step.Default;

            Equations = GetSystemEquations(false, SysParameters.Defaults, EqStep);
            EqN = Equations.N;
            
            TotIter = (long)(SysParameters.ModellingTime / EqStep);

            outArray = new double[TotIter, EqN];

            Equations.Solver.Init();
        }

        public override void Run()
        {
            for (int i = 0; i < TotIter; i++)
            {
                Equations.Solver.NexStep();

                for (int k = 0; k < EqN; k++)
                    outArray[i, k] = Equations.Solver.Solution[0, k];
            }
            WriteResults();
        }


        private void WriteResults()
        {
            StringBuilder output = new StringBuilder();

            string fileNameStart = Path.Combine(OutDir, Equations.ToFileName());

            double[] xt = new double[TotIter];
            double[] yt = new double[TotIter];
            double[] zt = new double[TotIter];

            double t = 0;
            for (int cnt = 0; cnt < TotIter; cnt++)
            {
                output.AppendFormat("{0:F5}", t);

                for (int k = 0; k < EqN; k++)
                    output.AppendFormat("\t{0:F15}", outArray[cnt, k]);

                output.Append("\n");

                xt[cnt] = outArray[cnt, 0];
                yt[cnt] = EqN > 1 ? outArray[cnt, 1] : 1;
                zt[cnt] = EqN > 2 ? outArray[cnt, 2] : 1;

                t += EqStep;
            }

            DataWriter.CreateDataFile(fileNameStart, output.ToString());
            Model3D.Create3daModelFile(fileNameStart + ".ply", xt, yt, zt);
            Sound.CreateWavFile(fileNameStart + ".wav", yt);
        }
    }
}
