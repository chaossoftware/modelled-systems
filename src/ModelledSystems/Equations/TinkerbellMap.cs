using ChaosSoft.NumericalMethods.Equations;

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
    /// Initializes a new instance of the <see cref="TinkerbellMap"/> class 
    /// with default system parameters values:<br/>
    /// a = 0.9, b = -0.6, c = 2.0, d = 0.5.
    /// </summary>
    public TinkerbellMap() : this(0.9, -0.6, 2.0, 0.5)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TinkerbellMap"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
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
    /// xₙ₊₁ = xₙ² − yₙ² + axₙ + byₙ<br/>
    /// yₙ₊₁ = 2xₙyₙ + cxₙ + dyₙ
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
    /// [0.001, 0.001].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0.001;
        }
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b", "c", "d"),
            A, B, C, D);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("tinkerbell", "a", "b", "c", "d"),
            A, B, C, D);
}
