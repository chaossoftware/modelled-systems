using System;
using ModelledSystems.Routines;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Globalization;

namespace ModelledSystems
{
    class Program {

        static Parameters parameters = new Parameters();
        static string OutDir = parameters.OutDir + "\\" + parameters.System;
        static Size Size = parameters.PicSize;


        public static void Main(string[] args) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            Init();
            PerformAction();
        }


        private static void Init()
        {
            if (!Directory.Exists(OutDir))
                Directory.CreateDirectory(OutDir);

            Console.Title = parameters.System + ": " + parameters.Action;
        }


        private static void PerformAction()
        {
            Console.Write("\nPerforming calculation...\n");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            GetRoutine().Run();

            timer.Stop();
            Console.WriteLine("Elapsed time: " + timer.Elapsed.ToString("mm\\:ss\\.ff"));
            Console.Read();
        }


        private static Routine GetRoutine()
        {
            Routine routine;
            switch(parameters.Action.ToLower())
            {
                case "signal":
                    routine = new SystemOut(OutDir, parameters.SystemParameters);
                    break;
                case "benettin_les":
                    routine = new BenettinSpectrum(OutDir, parameters.SystemParameters, parameters.Orthogonalization, parameters.Iterations);
                    break;
                case "benettin_lle":
                    routine = new BenettinLLE(OutDir, parameters.SystemParameters);
                    break;
                case "benettin_lle_param":
                    int paramIndexLle = Convert.ToInt32(parameters.ActionParams);
                    routine = new BenettinLLEParam(OutDir, parameters.SystemParameters, paramIndexLle);
                    break;
                case "le_map":
                    int mapX = Convert.ToInt32(parameters.ActionParams.Split('|')[0]);
                    int mapY = Convert.ToInt32(parameters.ActionParams.Split('|')[1]);
                    routine = new LyapunovExponentsMap(OutDir, parameters.SystemParameters, mapX, mapY);
                    break;
                case "bifurcation":
                    int paramIndex = Convert.ToInt32(parameters.ActionParams);
                    routine = new Bifurcation(OutDir, parameters.SystemParameters, paramIndex);
                    break;
                case "stefanski":
                    int p = Convert.ToInt32(parameters.ActionParams.Split('|')[0]);
                    double pstep = Convert.ToDouble(parameters.ActionParams.Split('|')[1]);
                    routine = new Stefanski(OutDir, parameters.SystemParameters, p, pstep);
                    break;
                default:
                    return null;

            }

            routine.Size = Size;
            return routine;
        }
    }
}