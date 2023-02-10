using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Lorenz system equations
/// 3 non-linear and 9 linear equations
/// Describes 
/// </summary>
public class RosslerAugmented : AugmentedEquations {

    protected const int EqCount = 9;
    private double a = 0.2;
    private double b = 0.2;
    private double c = 5.7;

    public RosslerAugmented() : base(EqCount)
    {
    }

    public RosslerAugmented(double a, double b, double c) : base(EqCount)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public override string Name => "Rossler Augmented";

    public override void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
    }

    public override void GetDerivatives(double[,] current, double[,] derivs) 
    {
        double x00 = current[0, 3] + current[0, 6];
        double x01 = current[0, 4] + current[0, 7];
        double x02 = current[0, 5] + current[0, 8];

        //Nonlinear Rossler equations:
        derivs[0, 0] = -x01 - x02;
        derivs[0, 1] = x00 + a * x01;
        derivs[0, 2] = b + x02 * (x00 - c);

        derivs[0, 3] = -current[0, 4] - current[0, 5];
        derivs[0, 4] = current[0, 3] + a * current[0, 4];
        derivs[0, 5] = b + current[0, 5] * (current[0, 3] - c);

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
        throw new NotImplementedException();

    public override string ToFileName() =>
        throw new NotImplementedException();
}