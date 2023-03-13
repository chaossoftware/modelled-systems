using System;
using System.IO;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Orthogonalization;
using ModelledSystems.Equations;
using ModelledSystems.Equations.Linearized;
using ScottPlot;

namespace ModelledSystems.Routines;

abstract class Routine
{
    protected Routine(string outDir, SystemCfg sysConfig)
    {
        OutDir = outDir;
        SysConfig = sysConfig;
        FileNameBase = Path.Combine(OutDir, SysConfig.Name);
    }

    protected string OutDir { get; }

    protected SystemCfg SysConfig { get; }

    public int PicWidth { get; set; }

    public int PicHeight { get; set; }

    protected string FileNameBase { get; } 

    public abstract void Run();

    protected Plot GetPlot(string xLabel, string yLabel)
    {
        Plot plot = new Plot(PicWidth, PicHeight);

        plot.XAxis.LabelStyle(fontSize: 12);
        plot.YAxis.LabelStyle(fontSize: 12);
        plot.XAxis.Label(xLabel);
        plot.YAxis.Label(yLabel);
        plot.Layout(padding: 0);

        return plot;
    }

    protected SystemBase GetSystemEquations(double[] sysParams)
    {
        SystemBase eq = SysConfig.Name.ToLowerInvariant() switch
        {
            "henon" => new HenonMap(),
            "henon_generalized" => new GeneralizedHenonMap(),
            "logistic" => new LogisticMap(),
            "tinkerbell" => new TinkerbellMap(),
            "lorenz" => new LorenzAttractor(),
            "rossler" => new Rossler(),
            "thomas" => new ThomasAttractor(),
            "halvorsen" => new HalvorsenAttractor(),
            "qi_chen" => new QiChenAttractor(),
            "chua" => new ChuaCircuit(),
            "stankevich" => new Stankevich(),
            "charo" => new CharoAttractor(),
            "henon_heiles" => new HenonHeiles(),
            "anischenko_nikolaev" => new AnishchenkoNikolaev(),
            "klein_baier" => new KleinBaier(),
            _ => throw new ArgumentException($"No such system: {SysConfig.Name}"),
        };

        eq.SetParameters(sysParams);
        return eq;
    }

    protected SystemBase GetLinearizedSystemEquations(double[] sysParams)
    {
        SystemBase eq = SysConfig.Name.ToLowerInvariant() switch
        {
            "lorenz" => new LorenzLinearized(),
            "rossler" => new RosslerLinearized(),
            "henon" => new HenonMapLinearized(),
            "henon_generalized" => new GeneralizedHenonLinearized(),
            "logistic" => new LogisticLinearized(),
            "tinkerbell" => new TinkerbellLinearized(),
            "henon_heiles" => new HenonHeilesLinearized(),
            "klein_baier" => new KleinBaierLinearized(),
            _ => throw new ArgumentException($"No such system: {SysConfig.Name}"),
        };

        eq.SetParameters(sysParams);
        return eq;
    }

    public SolverBase GetSolver(SystemBase eq)
    {
        double dt = SysConfig.Solver.Dt;

        return SysConfig.Solver.Name.ToLowerInvariant() switch
        {
            "discrete" => new DiscreteSolver(eq, dt),
            "rk5" => new RK5(eq, dt),
            _ => new RK4(eq, dt),
        };
    }

    public Type GetSolverType()
    {
        return SysConfig.Solver.Name.ToLowerInvariant() switch
        {
            "discrete" => typeof(DiscreteSolver),
            "rk5" => typeof(RK5),
            _ => typeof(RK4),
        };
    }

    public OrthogonalizationBase GetOrthogonalization(string ortType, int equationsCount)
    {
        return ortType.ToLowerInvariant() switch
        {
            "mgs" => new ModifiedGrammSchmidt(equationsCount),
            "cgs" => new ClassicGrammSchmidt(equationsCount),
            "hh" => new HouseholderTransformation(equationsCount),
            _ => throw new ArgumentException($"No such system: {SysConfig.Name}"),
        };
    }
}
