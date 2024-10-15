using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Stankevich, N. V., Shchegoleva, N. A., Sataev, I. R. & Kuznetsov, A. P. [2020] “Three-dimensional torus 
/// breakdown and chaos with two zero Lyapunov exponents in coupled radio-physical generators,
/// ” Journal of Computational and Nonlinear Dynamics 15, 111001.
/// </summary>
public class Stankevich : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    private double a;
    private double b;
    private double mu;
    private double o;
    private double om0;

    /// <summary>
    /// Initializes a new instance of the <see cref="Stankevich"/> class 
    /// with default system parameters values:<br/>
    /// α = 1.5, β = 0.04, μ = 4, ǫ = 0.02, ω₀ = 2π.
    /// </summary>
    public Stankevich() : this(1.5, 0.04, 4, 0.02, 2 * Math.PI)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Stankevich"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="mu"></param>
    /// <param name="koppa"></param>
    /// <param name="om0"></param>
    public Stankevich(double alpha, double beta, double mu, double koppa, double om0)
    {
        a = alpha;
        b = beta;
        this.mu = mu;
        o = koppa;
        this.om0 = om0;
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Stankevich attractor";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        mu = parameters[2];
        o = parameters[3];
        om0 = parameters[4];
    }

    /// <summary>
    /// dx/dt = y<br/>
    /// dy/dt = (α + z + x² − βx⁴)y − ω₀² x<br/>
    /// dz/dt = μ − z − ǫy²
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        double xPow2 = x * x;

        derivs[0] = y;
        derivs[1] = (a + z + xPow2 - b * xPow2 * xPow2) * y - om0 * om0 * x;
        derivs[2] = mu - z  - o * y * y;
    }

    public override string ToString() => 
        string.Format(
            SysFormat.GetInfoTemplate(Name, "α", "β", "μ", "ǫ", "ω0"),
            a, b, mu, o, om0);

    public string ToFileName() => 
        string.Format(
            SysFormat.GetFileTemplate("stankevich", "a", "b", "m", "o", "w0"),
            a, b, mu, o, om0);
}