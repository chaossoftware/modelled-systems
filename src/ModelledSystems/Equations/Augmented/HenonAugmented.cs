using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Algebra;
using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public sealed class HenonAugmented : IAugmentedEquations, IHasName, IHasParameters
{
    private double a = 1.4;
    private double b = 0.3;

    public HenonAugmented()
    {
    }

    public int EqCount { get; } = 6;

    public string Name { get; } = "Hénon Map Augmented";

    public double P { get; set; } = 0;

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
    }

    public void F(double t, double[] solution, double[] derivs)
    {
        double x00 = solution[2] + solution[4];
        double x01 = solution[3] + solution[5];

        //Nonlinear Henon map equations:
        derivs[0] = 1.0 - a * Numbers.FastPow2(x00) + x01;
        derivs[1] = b * x00;
        derivs[2] = 1 - a * Numbers.FastPow2(solution[2]) + solution[3];
        derivs[3] = b * solution[2];
        derivs[4] = (derivs[0] - derivs[2]) * Math.Exp(-P);
        derivs[5] = (derivs[1] - derivs[3]) * Math.Exp(-P);
    }

    public override string ToString() =>
        string.Format("{0}: a = {1:F1}; b = {2:F1}", Name, a, b);
}
