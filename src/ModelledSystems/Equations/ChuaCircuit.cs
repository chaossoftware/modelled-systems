using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Chua%27s_circuit">Chua circuit</see>.
/// </summary>
public class ChuaCircuit : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    private double a;
    private double b;
    private double c;
    private double d;
    private double e;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChuaCircuit"/> class 
    /// with default system parameters values:<br/>
    /// α = 15.6, β = 1, ς = 28, δ = −1.143, ε = −0.714.
    /// </summary>
    public ChuaCircuit() : this(15.6, 1, 28, -1.143, -0.714)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChuaCircuit"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="e"></param>
    public ChuaCircuit(double a, double b, double c, double d, double e)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.e = e;
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Chua's circuit";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
        d = parameters[3];
        e = parameters[4];
    }

    /// <summary>
    /// dx/dt = α(y − x − G(x))<br/>
    /// dy/dt = β(x − y + z)<br/>
    /// dz/dt = −ςy<br/>
    /// G(x) = εx + (δ − ε)(|x + 1| − |x − 1|)/2
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        derivs[0] = a * (y - x - G(x));
        derivs[1] = b * (x - y + z);
        derivs[2] = -c * y;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "α", "β", "ς", "δ", "ε"),
            a, b, c, d, e);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("chua", "a", "b", "c", "d", "e"),
            a, b, c, d, e);

    private double G(double x) =>
        e * x + (d - e) / 2 * (Math.Abs(x + 1) - Math.Abs(x - 1));
}