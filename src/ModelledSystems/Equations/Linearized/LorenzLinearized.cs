using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class LorenzLinearized : LorenzAttractor, ILinearizedOdeSys
{
    public LorenzLinearized() : base()
    {
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public LorenzLinearized(double sigma, double rho, double b) : base(sigma, rho, b)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < EqCount; i++)
        {
            double xl = linearization[0, i];
            double yl = linearization[1, i];
            double zl = linearization[2, i];

            derivs[0, i] = sg * (yl - xl);
            derivs[1, i] = xl * (r - solution[2]) - yl - solution[0] * zl;
            derivs[2, i] = xl * solution[1] + solution[0] * yl - b * zl;
        }
    }
}