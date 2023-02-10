using ChaosSoft.Core;
using System;

namespace ModelledSystems.Equations.Augmented;


/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public class HenonAugmented : AugmentedEquations
{
    protected const int EqCount = 6;

    private double a = 1.4;
    private double b = 0.3;

    public HenonAugmented() : base(EqCount)
    {
    }

    public HenonAugmented(double a, double b) : base(EqCount)
    {
        this.a = a;
        this.b = b;
    }

    public override string Name => "Henon Map Augmented";

    public override void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
    }

    public override void GetDerivatives(double[,] current, double[,] derivs) 
    {
        double x00 = current[0, 2] + current[0, 4];
        double x01 = current[0, 3] + current[0, 5];

        //Nonlinear Henon map equations:
        derivs[0, 0] = 1.0 - a * FastMath.Pow2(x00) + x01;
        derivs[0, 1] = b * x00;
        derivs[0, 2] = 1 - a * FastMath.Pow2(current[0, 2]) + current[0, 3];
        derivs[0, 3] = b * current[0, 2];
        derivs[0, 4] = (derivs[0, 0] - derivs[0, 2]) * Math.Exp(-p);
        derivs[0, 5] = (derivs[0, 1] - derivs[0, 3]) * Math.Exp(-p);
    }


    public override void SetInitialConditions(double[,] current) {
        //set diagonal and first n elements to 1
        for (int i = 0; i < Count; i++) 
        {
            current[0, i] = 1e-8;
        }
    }

    public override string ToString() =>
        string.Format("{0}: a = {1:F1}; b = {2:F1}", Name, a, b);

    public override string ToFileName() =>
        throw new NotImplementedException();
}
