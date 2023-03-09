namespace ModelledSystems.Equations.Linearized;

public class HenonHeilesLinearized : HenonHeiles
{
    private double xl, vxl, yl, vyl;

    public HenonHeilesLinearized() : base()
    {
        Rows += EqCount;
    }

    public override string Name => "Hénon & Heiles system (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        //Linearized equations:
        for (int i = 0; i < Count; i++)
        {
            xl = current[1, i];
            vxl = current[2, i];
            yl = current[3, i];
            vyl = current[4, i];

            derivs[1, i] = vxl;
            derivs[2, i] = -xl - 2 * (xl * current[0, 2] + current[0, 0] * yl);
            derivs[3, i] = vyl;
            derivs[4, i] = -yl - 2 * current[0, 0] * xl + 2 * current[0, 2] * yl;
        }
    }

    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++)
        {
            current[i + 1, i] = 0.1;
        }
    }
}