using ModelledSystems.Routines;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace ModelledSystems
{
    internal class Program
    {
        private const string ConfigFile = "systems_config.xml";
        private readonly Config _config;
        private readonly string _outDir;

        public Program()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            using FileStream fs = new FileStream(ConfigFile, FileMode.Open);
            _config = (Config)serializer.Deserialize(fs);
            _outDir = Path.Combine(_config.Task.Out.Dir, _config.Task.System);
        }
        
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine($"Version: {versionInfo.ProductVersion}");

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

            Console.Title = _config.Task.System + ": " + _config.Task.Action;
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
            double[] sysParams = _config.System.Params.Select(p => p.Value).ToArray();

            switch (_config.Task.Action.ToLowerInvariant())
            {
                case "signal":
                    routine = new SystemOut(_outDir, _config.System, _config.Task.Out.BinOutput);
                    break;
                case "bifurcation":
                    routine = new Bifurcation(_outDir, _config.System, 
                        _config.Routine.GetInt("paramIndex"), 
                        _config.Routine.GetInt("iterations"));
                    break;
                case "lle":
                    routine = new Lle(_outDir, _config.System);
                    break;
                case "lle_sync":
                    routine = new LleSync(_outDir, _config.System, 
                        _config.Routine.GetInt("iterations"), 
                        _config.Routine.GetDouble("convergeRatio"));
                    break;
                case "lle_by_param":
                    routine = new LleParam(_outDir, _config.System, 
                        _config.Routine.GetInt("paramIndex"), 
                        _config.Routine.GetInt("iterations"));
                    break;
                case "le_spec":
                    routine = new LeSpec(_outDir, _config.System, _config.Task.Orthogonalization);
                    break;
                case "le_spec_map":
                    routine = new LeSpecMap(_outDir, _config.System, 
                        _config.Routine.GetInt("param1Index"), 
                        _config.Routine.GetInt("param2Index"), 
                        _config.Routine.GetInt("iterations"),
                        _config.Task.Orthogonalization);
                    break;
                case "lyap_fractal":
                    routine = new LeFractal(_outDir, _config.System, 
                        _config.Routine.GetInt("paramIndex"), 
                        _config.Routine.GetInt("iterations"), 
                        _config.Routine.GetString("sequence"));
                    break;
                default:
                    return null;
            }

            routine.PicWidth = _config.Task.Out.PicWidth;
            routine.PicHeight = _config.Task.Out.PicHeight;
            return routine;
        }
    }
}