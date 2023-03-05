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
    /// ϛ = 10, r = 28, b = 8/3.
    /// </summary>
    public LorenzAttractor() : this(10, 28, 8d / 3d)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LorenzAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="sg"></param>
    /// <param name="r"></param>
    /// <param name="b"></param>
    public LorenzAttractor(double sg, double r, double b) : base(EqCount)
    {
        Sg = sg;
        R = r;
        B = b;
    }

    public double Sg { get; private set; }

    public double R { get; private set; }

    public double B { get; private set; }

    public override string Name => "Lorenz system";

    public override void SetParameters(params double[] parameters)
    {
        Sg = parameters[0];
        R = parameters[1];
        B = parameters[2];
    }

    /// <summary>
    /// dx/dt = ϛ(y − x)<br/>
    /// dy/dt = x(r − z) − y<br/>
    /// dz/dt = xy − bz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = Sg * (y - x);
        derivs[0, 1] = x * (R - z) - y;
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
            SysFormat.GetInfoTemplate(Name, "ϛ", "r", "b"),
            Sg, R, B);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("lorenz", "sg", "r", "b"),
            Sg, R, B);
}