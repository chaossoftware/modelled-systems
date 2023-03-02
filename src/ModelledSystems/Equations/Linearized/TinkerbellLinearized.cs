namespace ModelledSystems.Equations.Linearized;

public class TinkerbellLinearized : TinkerbellMap
{
    private double xl, yl;

    public TinkerbellLinearized() : base()
    {
        Rows += EqCount;
    }

    public TinkerbellLinearized(double a, double b, double c, double d) : base(a, b, c, d)
    {
        Rows += EqCount;
    }

    public override string Name => "Tinkerbell (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        double xMul2 = 2 * current[0, 0];
        double yMul2 = 2 * current[0, 1];

        //Linearized Tinkerbell map equations:
        for (int i = 0; i < Count; i++)
        {
            xl = current[1, i];
            yl = current[2, i];

            derivs[1, i] = xMul2 * xl + A * xl - yMul2 * yl + B * yl;
            derivs[2, i] = yMul2 * xl + C * xl + xMul2 * yl + D * yl;
        }
    }

    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++)
        {
            //for linearized maps
            current[i + 1, i] = 1.0;
        }
    }
}
