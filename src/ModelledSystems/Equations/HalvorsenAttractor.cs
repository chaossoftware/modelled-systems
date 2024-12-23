using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://www.vorillaz.com/halvorsen-attractor/">Halvorsen attractor</see>.
/// </summary>
public class HalvorsenAttractor : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    private double a;

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
    public HalvorsenAttractor(double a)
    {
        this.a = a;
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Halvorsen attractor";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
    }

    /// <summary>
    /// dx/dt = −ax − 4y −4z −y²<br/>
    /// dy/dt = −ay − 4z −4x −z²<br/>
    /// dz/dt = −az − 4x −4y −x²
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        double xMul4 = x * 4;
        double yMul4 = y * 4;
        double zMul4 = z * 4;

        derivs[0] = -a * x - yMul4 - zMul4 - y * y;
        derivs[1] = -a * y - zMul4 - xMul4 - z * z;
        derivs[2] = -a * z - xMul4 - yMul4 - x * x;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a"),
            a);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("halvorsen", "a"),
            a);
}
