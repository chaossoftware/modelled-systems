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
    /// Initializes a new instance of the <see cref="ThomasAttractor"/> class 
    /// with default system parameters values:<br/>
    /// b = 0.19.
    /// </summary>
    public ThomasAttractor() : this(0.19)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThomasAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="b"></param>
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
    /// dx/dt = sin(y) − bx<br/>
    /// dy/dt = sin(z) − by<br/>
    /// dz/dt = sin(x) − bz
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
    /// [1, 0, 1].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        current[0, 0] = 1;
        current[0, 1] = 0;
        current[0, 2] = 1;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "b"),
            B);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("thomas", "b"),
            B);
}
