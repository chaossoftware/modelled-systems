using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Rössler_attractor">Rössler attractor</see>.
/// </summary>
public class Rossler : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rossler"/> class 
    /// with default system parameters values:<br/>
    /// a = 0.2, b = 0.2, c = 5.7.
    /// </summary>
    public Rossler() : this(0.2, 0.2, 5.7)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rossler"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public Rossler(double a, double b, double c) : base(EqCount)
    {
        A = a;
        B = b;
        C = c;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public double C { get; private set; }

    public override string Name => "Rössler attractor";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
        C = parameters[2];
    }

    /// <summary>
    /// dx/dt = −y − z<br/>
    /// dy/dt = x + ay<br/>
    /// dz/dt = b + z(x − c)
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = -y - z;
        derivs[0, 1] = x + A * y;
        derivs[0, 2] = B + z * (x - C);
    }

    /// <summary>
    /// [0.01, 0.01, 0.01].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0.01;
        }
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b", "c"),
            A, B, C);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("rossler", "a", "b", "c"),
            A, B, C);
}