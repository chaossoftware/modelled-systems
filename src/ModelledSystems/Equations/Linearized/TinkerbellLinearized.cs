using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class TinkerbellLinearized : TinkerbellMap, ILinearizedOdeSys
{
    private double xl, yl;

    public TinkerbellLinearized() : base()
    {
    }

    public TinkerbellLinearized(double a, double b, double c, double d) : base(a, b, c, d)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        double xMul2 = 2 * solution[0];
        double yMul2 = 2 * solution[1];

        for (int i = 0; i < EqCount; i++)
        {
            xl = linearization[0, i];
            yl = linearization[1, i];

            derivs[0, i] = xMul2 * xl + a * xl - yMul2 * yl + b * yl;
            derivs[1, i] = yMul2 * xl + c * xl + xMul2 * yl + d * yl;
        }
    }
}
