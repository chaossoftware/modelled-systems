namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public class GeneralizedHenonLinearized : GeneralizedHenonMap
{
    public GeneralizedHenonLinearized() : base()
    {
        Rows += EqCount;
    }

    public GeneralizedHenonLinearized(double a, double b) : base(a, b)
    {
        Rows += EqCount;
    }

    public override string Name => "Generalized Henon Map (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        for (int i = 0; i < Count; i++)
        {
            derivs[1, i] = -2 * current[0, 1] * current[2, i] - B * current[3, i];
            derivs[2, i] = current[1, i];
            derivs[3, i] = current[2, i];
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
