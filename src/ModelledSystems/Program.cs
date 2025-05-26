using ModelledSystems.Configuration;
using ModelledSystems.Routines;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace ModelledSystems;

internal class Program
{
    private const string ConfigFile = "systems_config.xml";

    private readonly Config _config;
    private readonly string _outDir;

    public Program()
    {
        XmlSerializer serializer = new(typeof(Config));
        using FileStream fs = new(ConfigFile, FileMode.Open);
        _config = (Config)serializer.Deserialize(fs);
        _outDir = Path.Combine(_config.Out.Dir, _config.Task.System);
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

        Routine routine = GetRoutine();
        routine.ChartsConfig = _config.Out.Charts;
        routine.Run();

        Console.WriteLine(new string('_', Console.BufferWidth));
        Console.WriteLine("Elapsed time: " + timer.Elapsed.ToString("mm\\:ss\\.fff"));
        Console.Read();
    }

    private Routine GetRoutine() =>
        _config.Task.Action.ToLowerInvariant() switch
        {
            "signal" => new SystemOut(_outDir, _config, _config.Out.BinOutput),
            "bifurcation" => new Bifurcation(_outDir, _config,
                                _config.Routine.GetInt("paramIndex"),
                                _config.Routine.GetInt("iterations")),
            "lle_benettin" => new Lle(_outDir, _config),
            "lle_sync" => new LleSync(_outDir, _config,
                                _config.Routine.GetInt("iterations"),
                                _config.Routine.GetDouble("convergeRatio")),
            "lle_by_param" => new LleParam(_outDir, _config,
                                _config.Routine.GetInt("paramIndex"),
                                _config.Routine.GetInt("iterations")),
            "le_spec" => new LeSpec(_outDir, _config),
            "le_spec_map" => new LeSpecMap(_outDir, _config,
                                _config.Routine.GetInt("param1Index"),
                                _config.Routine.GetInt("param2Index"),
                                _config.Routine.GetInt("iterations")),
            "lyap_fractal" => new LeFractal(_outDir, _config,
                                _config.Routine.GetInt("paramIndex"),
                                _config.Routine.GetInt("iterations"),
                                _config.Routine.GetString("sequence")),
            _ => null,
        };
}