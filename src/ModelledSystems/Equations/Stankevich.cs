using ChaosSoft.Core.NumericalMethods.Equations;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="</see>.
/// </summary>
public class Stankevich : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z, xPow2;

    // <summary>
    /// For α = 1.5, β = 0.04, μ = 4, ǫ = 0.02, ω₀ = 2π.
    /// </summary>
    public Stankevich() : this(1.5, 0.04, 4, 0.02, 2 * Math.PI)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: a, b, c)</param>
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
    /// dy/dt = (α + z + x² − βx^4)y − ω₀² x<br/>
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
    /// Set all to 0.01.
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
        string.Format(GetInfoTemplate("α", "β", "μ", "ǫ", "ω0"),
        A, B, Mu, O, Om0);

    public override string ToFileName() => 
        string.Format(GetFileNameTemplate("a", "b", "m", "o", "w0"),
        A, B, Mu, O, Om0);
}