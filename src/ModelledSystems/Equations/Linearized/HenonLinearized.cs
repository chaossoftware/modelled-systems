namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Henon system equations
/// parameters: a, b
/// 2 linear and 4 non-linear equations
/// </summary>
public class HenonLinearized : HenonMap
{
    public HenonLinearized() : base()
    {
        Rows += EqCount;
    }

    public HenonLinearized(double a, double b) : base(a, b)
    {
        Rows += EqCount;
    }

    public override string Name => "Henon Map (linearized)";

    /// <summary>
    /// Nonlinear:<br/>
    /// dx/dt = 1 - ax^2 + y<br/>
    /// dy/dt = bx<para/>
    /// Linearized:<br/>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="derivs"></param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        base.GetDerivatives(current, derivs);

        double min2Mulax00 = -2.0 * A * current[0, 0]; // speed optimization

        //Linearized Henon map equations:
        derivs[1, 0] = min2Mulax00 * current[1, 0] + B * current[2, 0]; //dxdt[1, 0] = -2.0 * a * x[0, 0] * x[1, 0] + b * x[2, 0];
        derivs[1, 1] = min2Mulax00 * current[1, 1] + B * current[2, 1]; //dxdt[1, 1] = -2.0 * a * x[0, 0] * x[1, 1] + b * x[2, 1];
        derivs[2, 0] = current[1, 0];
        derivs[2, 1] = current[1, 1];
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
