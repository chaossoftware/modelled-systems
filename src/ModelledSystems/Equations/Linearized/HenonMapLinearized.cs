namespace ModelledSystems.Equations.Linearized;

public class HenonMapLinearized : HenonMap
{
    private double xl, yl;

    public HenonMapLinearized() : base()
    {
        Rows += EqCount;
    }

    public HenonMapLinearized(double a, double b) : base(a, b)
    {
        Rows += EqCount;
    }

    public override string Name => "Hénon map (linearized)";

    /// <summary>
    /// _xₙ₊₁ = -2axₙ * _xₙ + b * _yₙ
    /// _yₙ₊₁ = _xₙ
    /// </summary>
    /// <param name="current"></param>
    /// <param name="derivs"></param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        double min2AX = -2.0 * A * current[0, 0]; // speed optimization

        //Linearized Henon map equations:
        for (int i = 0; i < Count; i++)
        {
            xl = current[1, i];
            yl = current[2, i];

            derivs[1, i] = min2AX * xl + B * yl;
            derivs[2, i] = xl;
        }
    }

    /// <summary>
    /// Set diagonal elements to 1.
    /// </summary>
    /// <param name="current"></param>
    public override void SetInitialConditions(double[,] current)
    {
        base.SetInitialConditions(current);

        for (int i = 0; i < Count; i++)
        {
            current[i + 1, i] = 1;
        }
    }
}
