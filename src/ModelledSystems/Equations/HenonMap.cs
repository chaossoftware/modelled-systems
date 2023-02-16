using ChaosSoft.NumericalMethods.Equations;

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
    /// Initializes a new instance of the <see cref="HenonMap"/> class 
    /// with default system parameters values:<br/>
    /// a = 1.4, b = 0.3.
    /// </summary>
    public HenonMap() : this(1.4, 0.3)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HenonMap"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public HenonMap(double a, double b) : base(EqCount)
    {
        A = a;
        B = b;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public override string Name => "Hénon map";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
    }

    /// <summary>
    /// xₙ₊₁ = 1 — axₙ² + yₙ<br/>
    /// yₙ₊₁ = bxₙ
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
    /// [0, 0].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0;
        }
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b"),
            A, B);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("henon", "a", "b"),
            A, B);
}
