using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Algebra;
using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public sealed class LogisticAugmented : IAugmentedEquations, IHasName, IHasParameters
{
    private double a = 4;

    public LogisticAugmented()
    {
    }

    public int EqCount { get; } = 3;

    public string Name { get; } = "Logistic Map Augmented";

    public double P { get; set; } = 0;

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
    }

    public void F(double t, double[] solution, double[] derivs)
    {
        double x00 = solution[1] + solution[2];

        //Nonlinear Logistic map equations:
        derivs[0] = a * x00 - a * Numbers.FastPow2(x00);
        derivs[1] = a * solution[1] * (1 - solution[1]);
        derivs[2] = (derivs[0] - derivs[1]) * Math.Exp(-P);
    }

    public override string ToString() =>
        throw new NotImplementedException();
}
