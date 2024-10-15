using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Lorenz system equations
/// parameters: a, b, c
/// 3 non-linear and 9 linear equations
/// Describes 
/// </summary>
public sealed class RosslerLinearized : Rossler, ILinearizedOdeSys
{
    public RosslerLinearized() : base()
    {
    }

    public RosslerLinearized(double a, double b, double c) : base(a, b, c)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < EqCount; i++)
        {
            derivs[0, i] = -linearization[1, i] - linearization[2, i];
            derivs[1, i] = linearization[0, i] + a * linearization[1, i];
            derivs[2, i] = solution[2] * linearization[0, i] + solution[0] * linearization[2, i] - c * linearization[2, i];
        }
    }
}