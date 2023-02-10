using ChaosSoft.Core.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Hénon_map">Hénon map</see>.
/// </summary>
public class HenonMap : SystemBase
{
    protected const int EqCount = 2;

    private double x, y;

    /// <summary>
    /// For a = 1.4, b = 0.3.
    /// </summary>
    public HenonMap() : this(1.4, 0.3)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: a, b)</param>
    public HenonMap(double a, double b) : base(EqCount)
    {
        A = a;
        B = b;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public override string Name => "Henon map";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
    }

    /// <summary>
    /// dx/dt = 1 — ax² + y<br/>
    /// dy/dt = bx
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];

        derivs[0, 0] = 1 - A * x * x + y;
        derivs[0, 1] = B * x;
    }

    /// <summary>
    /// Set all to 0.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0;
        }
    }

    public override string ToString() => $"{Name}: a = {A:F3}; b = {B:F3}";

    public override string ToFileName() => $"{Name}_a={A:F3}_B={B:F3}";
}
