using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class AnishchenkoNikolaevLinearized : AnishchenkoNikolaev, ILinearizedOdeSys
{
    public AnishchenkoNikolaevLinearized() : base()
    {
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public AnishchenkoNikolaevLinearized(double gamma, double g, double d, double m) : base(gamma, g, d, m)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < EqCount; i++)
        {
            double xl = linearization[0, i];
            double yl = linearization[1, i];
            double zl = linearization[2, i];
            double phil = linearization[3, i];

            derivs[0, i] = yl + m * xl - solution[0] * phil - solution[3] * xl - 3 * d * solution[0] * solution[0] * xl;
            derivs[1, i] = -xl;
            derivs[2, i] = phil;
            derivs[3, i] = -gamma * phil + gamma * DPhi(solution[0], xl) - g * zl;
        }
    }

    private static double DPhi(double x, double xl)
    {
        if (x > 0)
        {
            return 2 * x * xl;
        }
        else if (x < 0)
        {
            return 0;
        }
        else
        {
            return 0;
            //double epsilon = 0.5;
            //double k = 2 * epsilon;
            //if (xl > 0 && xl < epsilon)
            //{
            //    return k * xl; // piecewise linear approximation
            //}
            //else
            //{
            //    return 0; //  xl < 0 or xl > epsilon (Ф(x) = 0)
            //}
        }
    }
}