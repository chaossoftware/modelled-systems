using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Tinkerbell_map">Tinkerbell map</see>.
/// </summary>
public class TinkerbellMap : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double a;
    protected double b;
    protected double c;
    protected double d;

    /// <summary>
    /// Initializes a new instance of the <see cref="TinkerbellMap"/> class 
    /// with default system parameters values:<br/>
    /// a = 0.9, b = -0.6013, c = 2.0, d = 0.5.
    /// </summary>
    public TinkerbellMap() : this(0.9, -0.6013, 2.0, 0.5)
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
    public TinkerbellMap(double a, double b, double c, double d)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    public int EqCount { get; } = 2;

    public string Name { get; } = "Tinkerbell map";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
        d = parameters[3];
    }

    /// <summary>
    /// xₙ₊₁ = xₙ² − yₙ² + axₙ + byₙ<br/>
    /// yₙ₊₁ = 2xₙyₙ + cxₙ + dyₙ
    /// </summary>
    /// <param name="solution">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];

        derivs[0] = x * x - y * y + a * x + b * y;
        derivs[1] = 2 * x * y + c * x + d * y;
    }

    public override string ToString() =>
        string.Format(SysFormat.GetInfoTemplate(Name, "a", "b", "c", "d"),
            a, b, c, d);

    public string ToFileName() =>
        string.Format(SysFormat.GetFileTemplate("tinkerbell", "a", "b", "c", "d"),
            a, b, c, d);
}
