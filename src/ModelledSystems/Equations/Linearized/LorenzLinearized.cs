namespace ModelledSystems.Equations.Linearized;

public class LorenzLinearized : LorenzAttractor
{
    private double xl, yl, zl;

    public LorenzLinearized() : base()
    {
        Rows += EqCount;
    }

    /// <summary>
    /// Params order: sigma, r, b
    /// </summary>
    /// <param name="step"></param>
    /// <param name="linearized"></param>
    /// <param name="vars"></param>
    public LorenzLinearized(double sigma, double rho, double b) : base(sigma, rho, b)
    {
        Rows += EqCount;
    }

    public override string Name => "Lorenz (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        //Linearized Lorenz equations:
        for (int i = 0; i < Count; i++)
        {
            xl = current[1, i];
            yl = current[2, i];
            zl = current[3, i];

            derivs[1, i] = Sigma * (yl - xl);
            derivs[2, i] = xl * (Rho - current[0, 2]) - yl - current[0, 0] * zl;
            derivs[3, i] = xl * current[0, 1] + current[0, 0] * yl - B * zl;
        }
    }

    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++)
        {
            current[i + 1, i] = 1.0;
        }
    }
}