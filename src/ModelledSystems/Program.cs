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
        private readonly Parameters _params;
        private readonly string _outDir;
        private readonly Size _size;

        public Program()
        {
            _params = new Parameters();
            _outDir = Path.Combine(_params.OutDir, _params.System);
            _size = _params.PicSize;
        }

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

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

            Console.Title = _params.System + ": " + _params.Action;
        }

        private void PerformAction()
        {
            Console.WriteLine("Processing task...");
            Stopwatch timer = Stopwatch.StartNew();

            GetRoutine().Run();

            Console.WriteLine(new string('_', Console.BufferWidth));
            Console.WriteLine("Elapsed time: " + timer.Elapsed.ToString("mm\\:ss\\.fff"));
            Console.Read();
        }

        private Routine GetRoutine()
        {
            Routine routine;
            switch (_params.Action.ToLower())
            {
                case "signal":
                    routine = new SystemOut(_outDir, _params.SystemParameters, _params.BinOutput);
                    break;
                case "benettin_les":
                    routine = new BenettinSpectrum(_outDir, _params.SystemParameters, _params.Orthogonalization, _params.Iterations);
                    break;
                case "benettin_lle":
                    routine = new BenettinLLE(_outDir, _params.SystemParameters);
                    break;
                case "benettin_lle_param":
                    int paramIndexLle = Convert.ToInt32(_params.ActionParams.Split('|')[0]);
                    int paramIterations = Convert.ToInt32(_params.ActionParams.Split('|')[1]);
                    routine = new BenettinLLEParam(_outDir, _params.SystemParameters, paramIndexLle, paramIterations);
                    break;
                case "le_map":
                    int mapX = Convert.ToInt32(_params.ActionParams.Split('|')[0]);
                    int mapY = Convert.ToInt32(_params.ActionParams.Split('|')[1]);
                    int mapParamIterations = Convert.ToInt32(_params.ActionParams.Split('|')[2]);
                    routine = new LesMap(_outDir, _params.SystemParameters, mapX, mapY, mapParamIterations);
                    break;
                case "bifurcation":
                    int paramIndex = Convert.ToInt32(_params.ActionParams.Split('|')[0]);
                    int paramIterationsB = Convert.ToInt32(_params.ActionParams.Split('|')[1]);
                    routine = new Bifurcation(_outDir, _params.SystemParameters, paramIndex, paramIterationsB);
                    break;
                case "synchronization_lle":
                    int p = Convert.ToInt32(_params.ActionParams.Split('|')[0]);
                    double pstep = Convert.ToDouble(_params.ActionParams.Split('|')[1]);
                    routine = new SynchronizationLLE(_outDir, _params.SystemParameters, p, pstep);
                    break;
                case "lyapunov_fractal":
                    int lefParamIndex = Convert.ToInt32(_params.ActionParams.Split('|')[0]);
                    int lefIterations = Convert.ToInt32(_params.ActionParams.Split('|')[1]);
                    string sequence = _params.ActionParams.Split('|')[2];
                    routine = new LeFractal(_outDir, _params.SystemParameters, lefParamIndex, lefIterations, sequence);
                    break;
                default:
                    return null;
            }

            routine.Size = _size;
            return routine;
        }
    }
}