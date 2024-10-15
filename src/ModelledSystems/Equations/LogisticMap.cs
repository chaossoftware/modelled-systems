using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Logistic_map">Logistic map</see>.
/// </summary>
public class LogisticMap : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double r;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogisticMap"/> class 
    /// with default system parameters values:<br/>
    /// R = 4.
    /// </summary>
    public LogisticMap() : this(4)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogisticMap"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="r"></param>
    public LogisticMap(double r)
    {
        this.r = r;
    }

    public int EqCount { get; } = 1;

    public string Name { get; } = "Logistic map";

    public void SetParameters(params double[] parameters)
    {
        r = parameters[0];
    }

    /// <summary>
    /// xₙ₊₁ = Rxₙ * (1 − xₙ)
    /// </summary>
    /// <param name="solution">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        derivs[0] = r * solution[0] * (1 - solution[0]);
    }

    public override string ToString() =>
        string.Format(SysFormat.GetInfoTemplate(Name, "R"), 
            r);

    public string ToFileName() =>
        string.Format(SysFormat.GetFileTemplate("logistic", "R"), 
            r);
}
