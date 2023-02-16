using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Lorenz_system">Lorenz attractor</see>.
/// </summary>
public class LorenzAttractor : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;

    /// <summary>
    /// Initializes a new instance of the <see cref="LorenzAttractor"/> class 
    /// with default system parameters values:<br/>
    /// ϛ = 10, ρ = 28, β = 8/3.
    /// </summary>
    public LorenzAttractor() : this(10, 28, 8d / 3d)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LorenzAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="sigma"></param>
    /// <param name="rho"></param>
    /// <param name="b"></param>
    public LorenzAttractor(double sigma, double rho, double b) : base(EqCount)
    {
        Sigma = sigma;
        Rho = rho;
        B = b;
    }

    public double Sigma { get; private set; }

    public double Rho { get; private set; }

    public double B { get; private set; }

    public override string Name => "Lorenz system";

    public override void SetParameters(params double[] parameters)
    {
        Sigma = parameters[0];
        Rho = parameters[1];
        B = parameters[2];
    }

    /// <summary>
    /// dx/dt = ϛ(y − x)<br/>
    /// dy/dt = x(ρ − z) − y<br/>
    /// dz/dt = xy − βz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = Sigma * (y - x);
        derivs[0, 1] = x * (Rho - z) - y;
        derivs[0, 2] = x * y - B * z;
    }

    /// <summary>
    /// [1, 1, 1].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 1;
        }
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "ϛ", "ρ", "β"),
            Sigma, Rho, B);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("lorenz", "sg", "r", "b"),
            Sigma, Rho, B);
}