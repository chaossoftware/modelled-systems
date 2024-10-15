using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Thomas%27_cyclically_symmetric_attractor">
/// Thomas' cyclically symmetric attractor</see>.
/// </summary>
public class ThomasAttractor : IOdeSys, IHasFileName, IHasParameters, IHasName
{ 
    private double b;

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
    public ThomasAttractor(double b)
    {
        this.b = b;
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Thomas attractor";

    public void SetParameters(params double[] parameters)
    {
        b = parameters[0];
    }

    /// <summary>
    /// dx/dt = sin(y) − bx<br/>
    /// dy/dt = sin(z) − by<br/>
    /// dz/dt = sin(x) − bz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        derivs[0] = Math.Sin(y) - b * x;
        derivs[1] = Math.Sin(z) - b * y;
        derivs[2] = Math.Sin(x) - b * z;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "b"),
            b);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("thomas", "b"),
            b);
}
