using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class HalvorsenLinearized : HalvorsenAttractor, ILinearizedOdeSys
{
    public HalvorsenLinearized() : base()
    {
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public HalvorsenLinearized(double a) : base(a)
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < 3; i++)
        {
            double xl = linearization[0, i];
            double yl = linearization[1, i];
            double zl = linearization[2, i];

            derivs[0, i] = -a * xl - (4 + 2 * solution[1]) * yl - 4 * zl;
            derivs[1, i] = -4 * xl - a * yl - (4 + 2 * solution[2]) * zl;
            derivs[2, i] = -(4 + 2 * solution[0]) * xl - 4 * yl - a * zl;
        }
    }
}