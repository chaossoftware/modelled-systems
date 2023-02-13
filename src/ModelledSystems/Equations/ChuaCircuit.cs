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
    /// For α = 15.6, β = 1, ς = 28, δ = -1.143, ε = -0.714.
    /// </summary>
    public ChuaCircuit() : this(15.6, 1, 28, -1.143, -0.714)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: α, β, ς, δ, ε)</param>
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

    public override string Name => "Chua circuit";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
        C = parameters[2];
        D = parameters[3];
        E = parameters[4];
    }

    /// <summary>
    /// dx/dt = α(y — x — G(x))<br/>
    /// dy/dt = β(x — y + z)<br/>
    /// dz/dt = —ςy<br/>
    /// G(x) = εx + (δ — ε)(|x + 1| — |x — 1|)/2
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
    /// 0.7, 0, 0.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        current[0, 0] = 0.7;
        current[0, 1] = 0;
        current[0, 2] = 0;
    }

    private double G(double x) =>
        E * x + (D - E) / 2 * (Math.Abs(x + 1) - Math.Abs(x - 1));

    public override string ToString() => $"{Name}: a = {A:F1}; b = {B:F1}; c = {C:F2}; d = {D:F2}; e = {E:F2}";

    public override string ToFileName() => $"{Name}_a={A:F1}_b={B:F1}_c={C:F2}_d={D:F2}_e={E:F2}";
}