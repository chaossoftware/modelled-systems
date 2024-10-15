using ChaosSoft.NumericalMethods.Ode.Linearized;

namespace ModelledSystems.Equations.Linearized;

public sealed class HenonHeilesLinearized : HenonHeiles, ILinearizedOdeSys
{
    private double xl, vxl, yl, vyl;

    public HenonHeilesLinearized() : base()
    {
    }


    public void F(double t, double[] solution, double[,] linearization, double[,] derivs)
    {
        for (int i = 0; i < EqCount; i++)
        {
            xl = linearization[0, i];
            vxl = linearization[1, i];
            yl = linearization[2, i];
            vyl = linearization[3, i];

            derivs[0, i] = vxl;
            derivs[1, i] = -xl - 2 * (xl * solution[2] + solution[0] * yl);
            derivs[2, i] = vyl;
            derivs[3, i] = -yl - 2 * solution[0] * xl + 2 * solution[2] * yl;
        }
    }
}