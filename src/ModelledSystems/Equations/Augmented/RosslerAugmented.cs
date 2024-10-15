using ChaosSoft.Core;
using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Lorenz system equations
/// 3 non-linear and 9 linear equations
/// Describes 
/// </summary>
public sealed class RosslerAugmented : IAugmentedEquations, IHasName, IHasParameters
{
    private double a = 0.2;
    private double b = 0.2;
    private double c = 5.7;

    public RosslerAugmented()
    {
    }

    public int EqCount { get; } = 9;

    public string Name { get; } = "Rossler Augmented";

    public double P { get; set; } = 0;

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
    }

    public void F(double t, double[] solution, double[] derivs)
    {
        double x00 = solution[3] + solution[6];
        double x01 = solution[4] + solution[7];
        double x02 = solution[5] + solution[8];

        //Nonlinear Rossler equations:
        derivs[0] = -x01 - x02;
        derivs[1] = x00 + a * x01;
        derivs[2] = b + x02 * (x00 - c);

        derivs[3] = -solution[4] - solution[5];
        derivs[4] = solution[3] + a * solution[4];
        derivs[5] = b + solution[5] * (solution[3] - c);

        derivs[6] = (derivs[0] - derivs[3]) * Math.Exp(-P);
        derivs[7] = (derivs[1] - derivs[4]) * Math.Exp(-P);
        derivs[8] = (derivs[2] - derivs[5]) * Math.Exp(-P);
    }

    public override string ToString() =>
        throw new NotImplementedException();
}