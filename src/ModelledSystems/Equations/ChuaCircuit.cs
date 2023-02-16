using ChaosSoft.NumericalMethods.Equations;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Chua%27s_circuit">Chua circuit</see>.
/// </summary>
public class ChuaCircuit : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;

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
    public ChuaCircuit(double a, double b, double c, double d, double e) : base(EqCount)
    {
        A = a;
        B = b;
        C = c;
        D = d;
        E = e;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public double C { get; private set; }

    public double D { get; private set; }

    public double E { get; private set; }

    public override string Name => "Chua's circuit";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
        C = parameters[2];
        D = parameters[3];
        E = parameters[4];
    }

    /// <summary>
    /// dx/dt = α(y − x − G(x))<br/>
    /// dy/dt = β(x − y + z)<br/>
    /// dz/dt = −ςy<br/>
    /// G(x) = εx + (δ − ε)(|x + 1| − |x − 1|)/2
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = A * (y - x - G(x));
        derivs[0, 1] = B * (x - y + z);
        derivs[0, 2] = -C * y;
    }

    /// <summary>
    /// [0.7, 0, 0]
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        current[0, 0] = 0.7;
        current[0, 1] = 0;
        current[0, 2] = 0;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "α", "β", "ς", "δ", "ε"),
            A, B, C, D, E);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("chua", "a", "b", "c", "d", "e"),
            A, B, C, D, E);

    private double G(double x) =>
        E * x + (D - E) / 2 * (Math.Abs(x + 1) - Math.Abs(x - 1));
}