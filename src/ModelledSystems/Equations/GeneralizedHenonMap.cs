using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://www.sciencedirect.com/science/article/abs/pii/037596019090283T">Generalized Hénon map</see>.
/// </summary>
public class GeneralizedHenonMap : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double a;
    protected double b;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralizedHenonMap"/> class 
    /// with default system parameters values:<br/>
    /// a = 1.9, b = 0.03.
    /// </summary>
    public GeneralizedHenonMap() : this(1.9, 0.03)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralizedHenonMap"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public GeneralizedHenonMap(double a, double b)
    {
        this.a = a;
        this.b = b;
    }

    public int EqCount { get; } = 3;
    
    public string Name { get; } = "Generalized Hénon map";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
    }

    /// <summary>
    /// Xₙ₊₁ = a − yₙ² − bzₙ<br/>
    /// yₙ₊₁ = xₙ<br/>
    /// zₙ₊₁ = yₙ
    /// </summary>
    /// <param name="solution">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        derivs[0] = a - solution[1] * solution[1] - b * solution[2];
        derivs[1] = solution[0];
        derivs[2] = solution[1];
    }

    public override string ToString() =>
        string.Format(SysFormat.GetInfoTemplate(Name, "a", "b"), 
            a, b);

    public string ToFileName() =>
        string.Format(SysFormat.GetFileTemplate("gen-henon", "a", "b"), 
            a, b);
}
