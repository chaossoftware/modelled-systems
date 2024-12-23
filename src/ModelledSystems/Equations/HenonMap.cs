using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Hénon_map">Hénon map</see>.
/// </summary>
public class HenonMap : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double a;
    protected double b;

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
    public HenonMap(double a, double b)
    {
        this.a = a;
        this.b = b;
    }

    public int EqCount { get; } = 2;

    public string Name { get; } = "Hénon map";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
    }

    /// <summary>
    /// xₙ₊₁ = 1 — axₙ² + yₙ<br/>
    /// yₙ₊₁ = bxₙ
    /// </summary>
    /// <param name="solution">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        derivs[0] = 1 - a * solution[0] * solution[0] + solution[1];
        derivs[1] = b * solution[0];
    }

    public override string ToString() =>
        string.Format(SysFormat.GetInfoTemplate(Name, "a", "b"),
            a, b);

    public string ToFileName() => 
        string.Format(SysFormat.GetFileTemplate("henon", "a", "b"), 
            a, b);
}
