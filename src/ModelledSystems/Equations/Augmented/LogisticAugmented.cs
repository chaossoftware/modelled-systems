using ChaosSoft.Core;
using System;

namespace ModelledSystems.Equations.Augmented;


/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public class LogisticAugmented : AugmentedEquations
{
    protected const int EqCount = 3;

    private double a = 4;

    public LogisticAugmented() : base(EqCount)
    {
    }

    public LogisticAugmented(double a) : base(EqCount)
    {
        this.a = a;
    }

    public override string Name => "Logistic Map Augmented";

    public override void SetParameters(params double[] parameters)
    {
        a = parameters[0];
    }

    public override void GetDerivatives(double[,] current, double[,] derivs) 
    {
        double x00 = current[0, 1] + current[0, 2];

        //Nonlinear Logistic map equations:
        derivs[0, 0] = a * x00 - a * FastMath.Pow2(x00);
        derivs[0, 1] = a * current[0, 1] * (1 - current[0, 1]);
        derivs[0, 2] = (derivs[0, 0] - derivs[0, 1]) * Math.Exp(-p);
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
        throw new NotImplementedException();

    public override string ToFileName() =>
        throw new NotImplementedException();
}
