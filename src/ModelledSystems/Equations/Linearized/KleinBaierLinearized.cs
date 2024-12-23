using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class KleinBaierLinearized : KleinBaier, ILinearizedOdeSys
{
    private double xl, yl, zl, wl;

    public KleinBaierLinearized() : base()
    {
    }

    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < EqCount; i++)
        {
            xl = linearization[0, i];
            yl = linearization[1, i];
            zl = linearization[2, i];
            wl = linearization[3, i];

            derivs[0, i] = -yl - a * zl - b * wl;
            derivs[1, i] = xl;
            derivs[2, i] = -c * wl - d * 2 * yl * solution[1];
            derivs[3, i] = c * zl - e * wl;
        }
    }
}