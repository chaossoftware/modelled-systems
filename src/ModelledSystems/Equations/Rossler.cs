using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Rössler_attractor">Rössler attractor</see>.
/// </summary>
public class Rossler : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double a;
    protected double b;
    protected double c;

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
    public Rossler(double a, double b, double c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Rössler attractor";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
    }

    /// <summary>
    /// dx/dt = −y − z<br/>
    /// dy/dt = x + ay<br/>
    /// dz/dt = b + z(x − c)
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        derivs[0] = -y - z;
        derivs[1] = x + a * y;
        derivs[2] = b + z * (x - c);
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b", "c"),
            a, b, c);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("rossler", "a", "b", "c"),
            a, b, c);
}