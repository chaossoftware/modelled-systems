using System;
using System.IO;
using ChaosSoft.NumericalMethods.Ode;
using ChaosSoft.NumericalMethods.Ode.Linearized;
using ChaosSoft.NumericalMethods.QrDecomposition;
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
    
    public double PicScale { get; set; }

    protected string FileNameBase { get; }

    protected string GetBaseFilePath(IOdeSys odeSys, double dt) =>
        Path.Combine(OutDir, $"{(odeSys as IHasFileName).ToFileName()}_st={dt:0.###}");

    public abstract void Run();

    protected Plot GetPlot(string xLabel, string yLabel)
    {
        Plot plot = new(PicWidth, PicHeight);

        plot.XAxis.LabelStyle(fontSize: 15);
        plot.YAxis.LabelStyle(fontSize: 15);
        plot.XAxis.TickLabelStyle(fontSize: 14);
        plot.YAxis.TickLabelStyle(fontSize: 14);
        plot.XAxis.Label(xLabel);
        plot.YAxis.Label(yLabel);
        plot.Layout(padding: 0);
        plot.Grid(enable: false);

        return plot;
    }

    protected void SavePlot(Plot plot, string path) =>
        plot.SaveFig(path, scale: PicScale);

    protected IOdeSys GetSystemEquations(double[] sysParams)
    {
        IOdeSys eq = SysConfig.Name.ToLowerInvariant() switch
        {
            "logistic" => new LogisticMap(),
            "henon" => new HenonMap(),
            "henon_generalized" => new GeneralizedHenonMap(),
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

        (eq as IHasParameters).SetParameters(sysParams);
        return eq;
    }

    protected ILinearizedOdeSys GetLinearizedSystemEquations(double[] sysParams)
    {
        ILinearizedOdeSys eq = SysConfig.Name.ToLowerInvariant() switch
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

        (eq as IHasParameters).SetParameters(sysParams);
        return eq;
    }

    public OdeSolverBase GetSolver(IOdeSys eq) =>
        SolverFactory.Get(SysConfig.Solver.Type, eq, SysConfig.Solver.Dt);

    public LinearizedOdeSolverBase GetLinearizedSolver(ILinearizedOdeSys eq) =>
        SolverFactory.Get(SysConfig.Solver.Type, eq, SysConfig.Solver.Dt);

    public IQrDecomposition GetOrthogonalization(string ortType, int equationsCount)
    {
        return ortType.ToLowerInvariant() switch
        {
            "mgs" => new ModifiedGrammSchmidt(equationsCount),
            "cgs" => new ClassicGrammSchmidt(equationsCount),
            "hh" => new HouseholderTransformation(equationsCount),
            _ => throw new ArgumentException($"No such system: {SysConfig.Name}"),
        };
    }

    public double[] GetInitialConditions() =>
        SysConfig.InitialConditions;
}
