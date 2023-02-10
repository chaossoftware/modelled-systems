namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Henon system equations
/// parameters: a, b
/// 2 linear and 4 non-linear equations
/// </summary>
public class LogisticLinearized : LogisticMap
{
    public LogisticLinearized() : base()
    {
        Rows += EqCount;
    }

    public LogisticLinearized(double r) : base(r)
    {
        Rows += EqCount;
    }

    public override string Name => "Logistic Map (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        //Linearized Logistic map equations:
        derivs[1, 0] = R - 2 * R * current[0, 0];
    }

    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        //set diagonal elements to 1
        for (int i = 0; i < Count; i++)
        {
            current[i + 1, i] = 1;
        }
    }
}
