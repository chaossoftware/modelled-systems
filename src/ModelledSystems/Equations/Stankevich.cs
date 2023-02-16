using ChaosSoft.NumericalMethods.Equations;
using System;
using System.Numerics;

namespace ModelledSystems.Equations;

/// <summary>
/// Stankevich, N. V., Shchegoleva, N. A., Sataev, I. R. & Kuznetsov, A. P. [2020] “Three-dimensional torus 
/// breakdown and chaos with two zero Lyapunov exponents in coupled radio-physical generators,
/// ” Journal of Computational and Nonlinear Dynamics 15, 111001.
/// </summary>
public class Stankevich : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z, xPow2;

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
    public Stankevich(double alpha, double beta, double mu, double koppa, double om0) : base(EqCount)
    {
        A = alpha;
        B = beta;
        Mu = mu;
        O = koppa;
        Om0 = om0;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public double Mu { get; private set; }

    public double O { get; private set; }

    public double Om0 { get; private set; }

    public override string Name => "Stankevich attractor";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
        Mu = parameters[2];
        O = parameters[3];
        Om0 = parameters[4];
    }

    /// <summary>
    /// dx/dt = y<br/>
    /// dy/dt = (α + z + x² − βx⁴)y − ω₀² x<br/>
    /// dz/dt = μ − z − ǫy²
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];
        
        xPow2 = x * x;

        derivs[0, 0] = y;
        derivs[0, 1] = (A + z + xPow2 - B * xPow2 * xPow2) * y - Om0 * Om0 * x;
        derivs[0, 2] = Mu - z  - O * y * y;
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
            SysFormat.GetInfoTemplate(Name, "α", "β", "μ", "ǫ", "ω0"),
            A, B, Mu, O, Om0);

    public override string ToFileName() => 
        string.Format(
            SysFormat.GetFileTemplate("stankevich", "a", "b", "m", "o", "w0"),
            A, B, Mu, O, Om0);
}