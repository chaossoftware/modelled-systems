using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://www.vorillaz.com/halvorsen-attractor/">Halvorsen attractor</see>.
/// </summary>
public class HalvorsenAttractor : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;
    private double xMul4, yMul4, zMul4;

    /// <summary>
    /// Initializes a new instance of the <see cref="HalvorsenAttractor"/> class 
    /// with default system parameters values:<br/>
    /// a = 1.4.
    /// </summary>
    public HalvorsenAttractor() : this(1.4)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HalvorsenAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    public HalvorsenAttractor(double a) : base(EqCount)
    {
        A = a;
    }

    public double A { get; private set; }

    public override string Name => "Halvorsen attractor";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
    }

    /// <summary>
    /// dx/dt = −ax − 4y −4z −y²<br/>
    /// dy/dt = −ay − 4z −4x −z²<br/>
    /// dz/dt = −az − 4x −4y −x²
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        xMul4 = x * 4;
        yMul4 = y * 4;
        zMul4 = z * 4;

        derivs[0, 0] = -A * x - yMul4 - zMul4 - y * y;
        derivs[0, 1] = -A * y - zMul4 - xMul4 - z * z;
        derivs[0, 2] = -A * z - xMul4 - yMul4 - x * x;
    }

    /// <summary>
    /// [−1.48, −1.51, 2.04].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        current[0, 0] = -1.48;
        current[0, 1] = -1.51;
        current[0, 2] = 2.04;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a"),
            A);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("halvorsen", "a"),
            A);
}
