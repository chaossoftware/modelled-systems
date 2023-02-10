namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Lorenz system equations
/// parameters: a, b, c
/// 3 non-linear and 9 linear equations
/// Describes 
/// </summary>
public class RosslerLinearized : Rossler
{
    public RosslerLinearized() : base()
    {
        Rows += EqCount;
    }

    public RosslerLinearized(double a, double b, double c) : base(a, b, c)
    {
        Rows += EqCount;
    }

    public override string Name => "Rossler (linearized)";

    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        //Linearized Rossler equations:
        for (int i = 0; i < Count; i++)
        {
            derivs[1, i] = -current[2, i] - current[3, i];
            derivs[2, i] = current[1, i] + A * current[2, i];
            derivs[3, i] = current[0, 2] * current[1, i] + current[0, 0] * current[3, i] - C * current[3, i];
        }
    }

    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++)
        {
            current[i + 1, i] = 0.01;
        }
    }
}