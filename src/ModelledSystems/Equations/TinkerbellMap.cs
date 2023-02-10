using ChaosSoft.Core.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Tinkerbell_map">Tinkerbell map</see>.
/// </summary>
public class TinkerbellMap : SystemBase
{
    protected const int EqCount = 2;

    private double x, y;

    /// <summary>
    /// For a = 0.9, b = -0.6, c = 2.0, d = 0.5.
    /// </summary>
    public TinkerbellMap() : this(0.9, -0.6, 2.0, 0.5)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: a, b, c, d)</param>
    public TinkerbellMap(double a, double b, double c, double d) : base(EqCount)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public double C { get; private set; }

    public double D { get; private set; }

    public override string Name => "Tinkerbell map";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
        C = parameters[2];
        D = parameters[3];
    }

    /// <summary>
    /// dx/dt = x² — y² + ax + by<br/>
    /// dy/dt = 2xy + cx + dy
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];

        derivs[0, 0] = x * x - y * y + A * x + B * y;
        derivs[0, 1] = 2 * x * y + C * x + D * y;
    }

    /// <summary>
    /// Set all to 0.001.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0.001;
        }
    }

    public override string ToString() => $"{Name}: a = {A:F3}; b = {B:F3}; c = {C:F3}; d = {D:F3}";

    public override string ToFileName() => $"{Name}_a={A:F3}_b={B:F3}_c={C:F3}_d={D:F3}";
}
