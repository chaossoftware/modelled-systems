using ChaosSoft.NumericalMethods.Equations;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Thomas%27_cyclically_symmetric_attractor">
/// Thomas' cyclically symmetric attractor</see>.
/// </summary>
public class ThomasAttractor : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;

    /// <summary>
    /// For b = 0.19.
    /// </summary>
    public ThomasAttractor() : this(0.19)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: b)</param>
    public ThomasAttractor(double b) : base(EqCount)
    {
        B = b;
    }

    public double B { get; private set; }

    public override string Name => "Thomas attractor";

    public override void SetParameters(params double[] parameters)
    {
        B = parameters[0];
    }

    /// <summary>
    /// dx/dt = Sin(y) — bx<br/>
    /// dy/dt = Sin(z) — by<br/>
    /// dz/dt = Sin(x) — bz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = Math.Sin(y) - B * x;
        derivs[0, 1] = Math.Sin(z) - B * y;
        derivs[0, 2] = Math.Sin(x) - B * z;
    }

    /// <summary>
    /// 1, 0, 1.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        current[0, 0] = 1;
        current[0, 1] = 0;
        current[0, 2] = 1;
    }

    public override string ToString() => $"{Name}: b = {B:F2}";

    public override string ToFileName() => $"{Name}_b={B:F2}";
}
