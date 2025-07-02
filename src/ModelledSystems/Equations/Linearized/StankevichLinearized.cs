using ChaosSoft.NumericalMethods.Ode.Linearized;
using System;

namespace ModelledSystems.Equations.Linearized;

public sealed class StankevichLinearized : Stankevich, ILinearizedOdeSys
{
    public StankevichLinearized() : base()
    {
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public StankevichLinearized(double alpha, double beta, double mu, double koppa, double om0) 
        : base(alpha, beta, mu, koppa, om0)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        double x = solution[0];
        double y = solution[1];

        for (int i = 0; i < EqCount; i++)
        {
            double xl = linearization[0, i];
            double yl = linearization[1, i];
            double zl = linearization[2, i];

            derivs [0, i] = yl;
            derivs [1, i] = (2 * x * y - 4 * b * Math.Pow(x, 3) * y - om0 * om0) * xl + (a + solution[2] + x * x - b * Math.Pow(x, 4)) * yl + y * zl;
            derivs [2, i] = -2 * o * y * yl - zl;
        }
    }
}