using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class HenonMapLinearized : HenonMap, ILinearizedOdeSys
{
    public HenonMapLinearized() : base()
    {
    }

    public HenonMapLinearized(double a, double b) : base(a, b)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        double min2AX = -2.0 * a * solution[0]; // speed optimization

        for (int i = 0; i < EqCount; i++)
        {
            double xl = linearization[0, i];

            derivs[0, i] = min2AX * xl + b * linearization[1, i];
            derivs[1, i] = xl;
        }
    }
}
