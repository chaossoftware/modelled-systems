using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Anishchenko, V. S. & Nikolaev, S. M. [2005] “Generator of quasi-periodic oscillations featuring 
/// two-dimensional torus doubling bifurcations,” Technical Physics Letters 31, 853–855, doi:10.1134/1.2121837.
/// </summary>
public class AnishchenkoNikolaev : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double gamma;
    protected double g;
    protected double d;
    protected double m;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnishchenkoNikolaev"/> class 
    /// with default system parameters values:<br/>
    /// γ = 0.2, g = 0.43, d = 0.001, m = 0.0809
    /// </summary>
    public AnishchenkoNikolaev() : this(0.2, 0.43, 0.001, 0.0809)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnishchenkoNikolaev"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="gamma"></param>
    /// <param name="g"></param>
    /// <param name="d"></param>
    /// <param name="m"></param>
    public AnishchenkoNikolaev(double gamma, double g, double d, double m)
    {
        this.gamma = gamma;
        this.g = g;
        this.d = d;
        this.m = m;
    }

    public int EqCount { get; } = 4;

    public string Name { get; } = "Anishchenko & Nikolaev attractor";

    public void SetParameters(params double[] parameters)
    {
        gamma = parameters[0];
        g = parameters[1];
        d = parameters[2];
        m = parameters[3];
    }

    /// <summary>
    /// dx/dt = mx + y - xφ - dx³<br/>
    /// dy/dt = -x<br/>
    /// dz/dt = φ<br/>
    /// dφ/dt = -γφ + γФ(x) - gz<para/>
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
        double phi = solution[3];

        derivs[0] = m * x + y - x * phi - d * x * x * x;
        derivs[1] = -x;
        derivs[2] = phi;
        derivs[3] = -gamma * phi + gamma * Phi(x) - g * z;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "γ", "g", "d", "m"),
            gamma, g, d, m);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("anishchenko-nikolaev", "gamma", "g", "d", "m"),
            gamma, g, d, m);

    private static double Phi(double x) =>
        x > 0 ? x * x : 0;
}
