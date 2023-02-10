using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Lorenz system equations
/// 3 linear and 9 non-linear equations
/// Describes fluid flow
/// </summary>
public class LorenzAugmented : AugmentedEquations
{
    protected const int EqCount = 9;

    private double sg = 10.0;
    private double r = 28.0;
    private double b = 8.0 / 3.0;

    public LorenzAugmented() : base(EqCount) 
    {
    }

    public LorenzAugmented(double sigma, double r, double b) : base(EqCount)
    {
        this.sg = sigma;
        this.r = r;
        this.b = b;
    }

    public override string Name => "Lorenz Augmented";

    public override void SetParameters(params double[] parameters)
    {
        sg = parameters[0];
        r = parameters[1];
        b = parameters[2];
    }

    public override void GetDerivatives(double[,] current, double[,] derivs) 
    {
        double x00 = current[0, 3] + current[0, 6];
        double x01 = current[0, 4] + current[0, 7];
        double x02 = current[0, 5] + current[0, 8];

        //Nonlinear Lorenz equations:
        derivs[0, 0] = sg * (x01 - x00);
        derivs[0, 1] = -x00 * x02 + r * x00 - x01;
        derivs[0, 2] = x00 * x01 - b * x02;

        derivs[0, 3] = sg * (current[0, 4] - current[0, 3]);
        derivs[0, 4] = -current[0, 3] * current[0, 5] + r * current[0, 3] - current[0, 4];
        derivs[0, 5] = current[0, 3] * current[0, 4] - b * current[0, 5];

        derivs[0, 6] = (derivs[0, 0] - derivs[0, 3]) * Math.Exp(-p);
        derivs[0, 7] = (derivs[0, 1] - derivs[0, 4]) * Math.Exp(-p);
        derivs[0, 8] = (derivs[0, 2] - derivs[0, 5]) * Math.Exp(-p);
    }

    public override void SetInitialConditions(double[,] current) 
    {
        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++) 
        {
            current[0, i] = 1e-8;

        }
    }

    public override string ToString() =>
        string.Format("{0}: sigma = {1:F3}; rho = {2:F3}; b = {3:F3}", Name, sg, r, b);

    public override string ToFileName() =>
        throw new NotImplementedException();
}