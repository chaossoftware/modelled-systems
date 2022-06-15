using ModelledSystems.Routines;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ModelledSystems
{
    internal class Program
    {
        private readonly Parameters _parameters;
        private readonly string _outDir;
        private readonly Size _size;

        public Program()
        {
            _parameters = new Parameters();
            _outDir = _parameters.OutDir + "\\" + _parameters.System;
            _size = _parameters.PicSize;
        }

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            var program = new Program();

            program.Init();
            program.PerformAction();
        }

        private void Init()
        {
            if (!Directory.Exists(_outDir))
            {
                Directory.CreateDirectory(_outDir);
            }

            Console.Title = _parameters.System + ": " + _parameters.Action;
        }

        private void PerformAction()
        {
            Console.Write("\nPerforming calculation...\n");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            GetRoutine().Run();

            timer.Stop();
            Console.WriteLine("Elapsed time: " + timer.Elapsed.ToString("mm\\:ss\\.ff"));
            Console.Read();
        }

        private Routine GetRoutine()
        {
            Routine routine;
            switch (_parameters.Action.ToLower())
            {
                case "signal":
                    routine = new SystemOut(_outDir, _parameters.SystemParameters);
                    break;
                case "benettin_les":
                    routine = new BenettinSpectrum(_outDir, _parameters.SystemParameters, _parameters.Orthogonalization, _parameters.Iterations);
                    break;
                case "benettin_lle":
                    routine = new BenettinLLE(_outDir, _parameters.SystemParameters);
                    break;
                case "benettin_lle_param":
                    int paramIndexLle = Convert.ToInt32(_parameters.ActionParams);
                    routine = new BenettinLLEParam(_outDir, _parameters.SystemParameters, paramIndexLle);
                    break;
                case "le_map":
                    int mapX = Convert.ToInt32(_parameters.ActionParams.Split('|')[0]);
                    int mapY = Convert.ToInt32(_parameters.ActionParams.Split('|')[1]);
                    routine = new LyapunovExponentsMap(_outDir, _parameters.SystemParameters, mapX, mapY);
                    break;
                case "bifurcation":
                    int paramIndex = Convert.ToInt32(_parameters.ActionParams);
                    routine = new Bifurcation(_outDir, _parameters.SystemParameters, paramIndex);
                    break;
                case "stefanski":
                    int p = Convert.ToInt32(_parameters.ActionParams.Split('|')[0]);
                    double pstep = Convert.ToDouble(_parameters.ActionParams.Split('|')[1]);
                    routine = new Stefanski(_outDir, _parameters.SystemParameters, p, pstep);
                    break;
                default:
                    return null;
            }

            routine.Size = _size;
            return routine;
        }
    }
}