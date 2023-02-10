using ChaosSoft.Core.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Logistic_map">Logistic map</see>.
/// </summary>
public class LogisticMap : SystemBase
{
    protected const int EqCount = 1;

    private double x;

    /// <summary>
    /// For r = 4.
    /// </summary>
    public LogisticMap() : this(4)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: r)</param>
    public LogisticMap(double r) : base(EqCount)
    {
        R = r;
    }

    public double R { get; private set; }

    public override string Name => "Logistic map";

    public override void SetParameters(params double[] parameters)
    {
        R = parameters[0];
    }

    /// <summary>
    /// dx/dt = rx * (1 — x)
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];

        derivs[0, 0] = R * x * (1 - x);
    }

    /// <summary>
    /// Set all to 0.1.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0.1;
        }
    }

    public override string ToString() => $"{Name}: r = {R:F3}";

    public override string ToFileName() => $"{Name}_r={R:F3}";
}
