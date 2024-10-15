using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Henon system equations
/// parameters: a, b
/// 2 linear and 4 non-linear equations
/// </summary>
public sealed  class LogisticLinearized : LogisticMap, ILinearizedOdeSys
{
    public LogisticLinearized() : base()
    {
    }

    public LogisticLinearized(double r) : base(r)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        derivs[0, 0] = r - 2 * r * solution[0];
    }
}
