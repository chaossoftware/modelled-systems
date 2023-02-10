namespace ModelledSystems.Equations.Linearized;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public class TinkerbellLinearized : TinkerbellMap
{

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

        double x00Mul2 = 2 * current[0, 0];
        double x01Mul2 = 2 * current[0, 1];

        //Linearized Tinkerbell map equations:
        derivs[1, 0] = x00Mul2 * current[1, 0] + A * current[1, 0] - x01Mul2 * current[2, 0] + B * current[2, 0]; //2 * x[0, 0] * x[1, 0] + a * x[1, 0] - 2 * x[0, 1] * x[2, 0] + b * x[2, 0]
        derivs[1, 1] = x00Mul2 * current[1, 1] + A * current[1, 1] - x01Mul2 * current[2, 1] + B * current[2, 1]; //2 * x[0, 0] * x[1, 1] + a * x[1, 1] - 2 * x[0, 1] * x[2, 1] + b * x[2, 1]
        derivs[2, 0] = x01Mul2 * current[1, 0] + C * current[1, 0] + x00Mul2 * current[2, 0] + D * current[2, 0]; //2 * x[0, 1] * x[1, 0] + c * x[1, 0] + 2 * x[0, 0] * x[2, 0] + d * x[2, 0]
        derivs[2, 1] = x01Mul2 * current[1, 1] + C * current[1, 1] + x00Mul2 * current[2, 1] + D * current[2, 1]; //2 * x[0, 1] * x[1, 1] + c * x[1, 1] + 2 * x[0, 0] * x[2, 1] + d * x[2, 1]
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
