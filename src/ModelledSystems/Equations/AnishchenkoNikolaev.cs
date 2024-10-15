using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Anishchenko, V. S. & Nikolaev, S. M. [2005] “Generator of quasi-periodic oscillations featuring 
/// two-dimensional torus doubling bifurcations,” Technical Physics Letters 31, 853–855, doi:10.1134/1.2121837.
/// </summary>
public class AnishchenkoNikolaev : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    private double a;
    private double b;
    private double d;
    private double mu;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnishchenkoNikolaev"/> class 
    /// with default system parameters values:<br/>
    /// α = 0.2, β = 0.43, δ = 0.001, μ = 0.0809
    /// </summary>
    public AnishchenkoNikolaev() : this(0.2, 0.43, 0.001, 0.0809)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnishchenkoNikolaev"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="d"></param>
    /// <param name="mu"></param>
    public AnishchenkoNikolaev(double a, double b, double d, double mu)
    {
        this.a = a;
        this.b = b;
        this.d = d;
        this.mu = mu;
    }

    public int EqCount { get; } = 4;

    public string Name { get; } = "Anishchenko & Nikolaev attractor";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        d = parameters[2];
        mu = parameters[3];
    }

    /// <summary>
    /// dx/dt = −y<br/>
    /// dy/dt = x + μy − yw − δy³<br/>
    /// dz/dt = w<br/>
    /// dw/dt = −βz − αw + αФ(y)<para/>
    /// Ф(x) = I(x)x²<br/>
    /// I(x) = 1, x > 0; 0, x ≤ 0
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];
        double w = solution[3];

        derivs[0] = -y;
        derivs[1] = x + mu * y - y * w - d * y * y * y;
        derivs[2] = w;
        derivs[3] = -b * z - a * w + a * Fx(y);
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "α", "β", "δ", "μ"),
            a, b, d, mu);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("anishchenko-nikolaev", "a", "b", "d", "mu"),
            a, b, d, mu);

    private static double Fx(double x) =>
        x > 0 ? x * x : 0;
}
