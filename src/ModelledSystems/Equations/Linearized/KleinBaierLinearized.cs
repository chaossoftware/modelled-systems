namespace ModelledSystems.Equations.Linearized;

public class KleinBaierLinearized : KleinBaier
{
    private double xl, yl, zl, wl;

    public KleinBaierLinearized() : base()
    {
        Rows += EqCount;
    }

    public override string Name => "Klein & Baier (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        //Linearized equations:
        for (int i = 0; i < Count; i++)
        {
            xl = current[1, i];
            yl = current[2, i];
            zl = current[3, i];
            wl = current[4, i];

            derivs[1, i] = -yl - A * zl - B * wl;
            derivs[2, i] = xl;
            derivs[3, i] = -C * wl - D * 2 * yl * current[0, 1];
            derivs[4, i] = C * zl - E * wl;
        }
    }

    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++)
        {
            current[i + 1, i] = 1;
        }
    }
}