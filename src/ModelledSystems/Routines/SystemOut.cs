using MathLib.IO;
using MathLib.MathMethods.Solvers;
using MathLib.Transform;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines
{
    internal class SystemOut : Routine
    {
        private readonly long totalIterations;
        private readonly int eqN;
        private readonly double eqStep;
        private readonly double[,] outArray;
        private readonly SystemEquations equations;

        public SystemOut(string outDir, SystemParameters systemParameters) : base (outDir, systemParameters)
        {
            eqStep = SysParameters.Step.Default;

            equations = GetSystemEquations(false, SysParameters.Defaults, eqStep);
            eqN = equations.EquationsCount;
            
            totalIterations = (long)(SysParameters.ModellingTime / eqStep);

            outArray = new double[totalIterations, eqN];

            equations.Solver.Init();
        }

        public override void Run()
        {
            for (int i = 0; i < totalIterations; i++)
            {
                equations.Solver.NexStep();

                for (int k = 0; k < eqN; k++)
                    outArray[i, k] = equations.Solver.Solution[0, k];
            }

            WriteResults();
        }

        private void WriteResults()
        {
            StringBuilder output = new StringBuilder();

            string fileNameStart = Path.Combine(OutDir, equations.ToFileName());

            double[] xt = new double[totalIterations];
            double[] yt = new double[totalIterations];
            double[] zt = new double[totalIterations];

            double t = 0;

            for (int cnt = 0; cnt < totalIterations; cnt++)
            {
                output.AppendFormat("{0:F5}", t);

                for (int k = 0; k < eqN; k++)
                    output.AppendFormat("\t{0:F15}", outArray[cnt, k]);

                output.Append("\n");

                xt[cnt] = outArray[cnt, 0];
                yt[cnt] = eqN > 1 ? outArray[cnt, 1] : 1;
                zt[cnt] = eqN > 2 ? outArray[cnt, 2] : 1;

                t += eqStep;
            }

            DataWriter.CreateDataFile(fileNameStart, output.ToString());
            Model3D.Create3daModelFile(fileNameStart + ".3da", xt, yt, zt);
            Sound.CreateWavFile(fileNameStart + ".wav", yt);
        }
    }
}
