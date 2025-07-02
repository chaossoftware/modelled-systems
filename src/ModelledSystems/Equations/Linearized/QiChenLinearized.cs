using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class QiChenLinearized : QiChenAttractor, ILinearizedOdeSys
{
    public QiChenLinearized() : base()
    {
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public QiChenLinearized(double a, double b, double c) : base(a, b, c)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < 3; i++)
        {
            double xl = linearization[0, i];
            double yl = linearization[1, i];
            double zl = linearization[2, i];

            derivs[0, i] = -a * xl + (a + solution[2]) * yl + solution[1] * zl;
            derivs[1, i] = (c - solution[2]) * xl - yl - solution[0] * zl;
            derivs[2, i] = solution[1] * xl + solution[0] * yl - b * zl;
        }
    }
}