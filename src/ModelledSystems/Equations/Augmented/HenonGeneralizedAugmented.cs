using System;

namespace ModelledSystems.Equations.Augmented;


/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public class HenonGeneralizedAugmented : AugmentedEquations
{
    protected const int EqCount = 9;

    private double a = 1.9;
    private double b = 0.03;

    public HenonGeneralizedAugmented() : base(EqCount)
    {
    }

    public override string Name => "Generalized Henon Map Augmented";

    public override void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
    }

    public override void GetDerivatives(double[,] current, double[,] derivs) 
    {
        double x00 = current[0, 3] + current[0, 6];
        double x01 = current[0, 4] + current[0, 7];
        double x02 = current[0, 5] + current[0, 8];

        //Nonlinear Henon map equations:
        derivs[0, 0] = a - x01 * x01 - b * x02;
        derivs[0, 1] = x00;
        derivs[0, 2] = x01;

        derivs[0, 3] = a - current[0, 4] * current[0, 4] - b * current[0, 5];
        derivs[0, 4] = current[0, 3];
        derivs[0, 5] = current[0, 4];

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
        string.Format("{0}: a = {1:F1}; b = {2:F1}", Name, a, b);

    public override string ToFileName() =>
        string.Format("{0}_a={1:F1}_b={2:F1}", Name, a, b);
}
