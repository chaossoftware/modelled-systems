namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Lorenz system equations
/// parameters: sigma, r, b
/// 3 linear and 9 non-linear equations
/// Describes fluid flow
/// </summary>
public class LorenzLinearized : LorenzAttractor
{
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
            derivs[1, i] = Sigma * (current[2, i] - current[1, i]);
            derivs[2, i] = (Rho - current[0, 2]) * current[1, i] - current[2, i] - current[0, 0] * current[3, i];
            derivs[3, i] = current[0, 1] * current[1, i] + current[0, 0] * current[2, i] - B * current[3, i];
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