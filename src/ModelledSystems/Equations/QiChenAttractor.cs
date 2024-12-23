using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/f3263f7618879.560aea484fce0.jpg">
/// Qi-Chen attractor</see>.<br/>
/// (<see href="https://www.behance.net/gallery/7618879/Strange-Attractors">All attractors</see>)
/// </summary>
public class QiChenAttractor : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    private double a;
    private double b;
    private double c;

    /// <summary>
    /// Initializes a new instance of the <see cref="QiChenAttractor"/> class 
    /// with default system parameters values:<br/>
    /// α = 38, β = 8/3, ς = 80.
    /// </summary>
    public QiChenAttractor() : this(38, 8/3, 80)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QiChenAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public QiChenAttractor(double a, double b, double c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Qi-Chen attractor";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
    }

    /// <summary>
    /// dx/dt = α(y − x) + yz<br/>
    /// dy/dt = ςx − y − xz<br/>
    /// dz/dt = xy − βz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        derivs[0] = a * (y - x) + y * z;
        derivs[1] = c * x - y - x * z;
        derivs[2] = x * y - b * z;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "α", "β", "ς"),
            a, b, c);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("qi-chen", "a", "b", "c"),
            a, b, c);
}
