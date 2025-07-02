using ChaosSoft.NumericalMethods.Ode.Linearized;
using System;

namespace ModelledSystems.Equations.Linearized;

public sealed class ThomasLinearized : ThomasAttractor, ILinearizedOdeSys
{
    public ThomasLinearized() : base()
    {
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public ThomasLinearized(double b) : base(b)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < 3; i++)
        {
            double xl = linearization[0, i];
            double yl = linearization[1, i];
            double zl = linearization[2, i];

            derivs[0, i] = Math.Cos(solution[1]) * yl - b * xl;
            derivs[1, i] = Math.Cos(solution[2]) * zl - b * yl;
            derivs[2, i] = Math.Cos(solution[0]) * xl - b * zl;
        }
    }
}