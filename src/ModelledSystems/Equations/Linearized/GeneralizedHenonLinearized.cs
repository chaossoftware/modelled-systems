using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public sealed class GeneralizedHenonLinearized : GeneralizedHenonMap, ILinearizedOdeSys
{
    public GeneralizedHenonLinearized() : base()
    {
    }

    public GeneralizedHenonLinearized(double a, double b) : base(a, b)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < EqCount; i++)
        {
            derivs[0, i] = -2 * solution[1] * linearization[1, i] - b * linearization[2, i];
            derivs[1, i] = linearization[0, i];
            derivs[2, i] = linearization[1, i];
        }
    }
}
