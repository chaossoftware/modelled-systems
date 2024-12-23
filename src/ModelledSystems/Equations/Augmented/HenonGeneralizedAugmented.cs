using ChaosSoft.Core;
using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public sealed class HenonGeneralizedAugmented : IAugmentedEquations, IHasName, IHasParameters
{
    private double a = 1.9;
    private double b = 0.03;

    public HenonGeneralizedAugmented()
    {
    }

    public int EqCount { get; } = 9;

    public string Name { get; } = "Generalized Hénon Map Augmented";

    public double P { get; set; } = 0;

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
    }

    public void F(double t, double[] solution, double[] derivs)
    {
        double x00 = solution[3] + solution[6];
        double x01 = solution[4] + solution[7];
        double x02 = solution[5] + solution[8];

        //Nonlinear Henon map equations:
        derivs[0] = a - x01 * x01 - b * x02;
        derivs[1] = x00;
        derivs[2] = x01;

        derivs[3] = a - solution[4] * solution[4] - b * solution[5];
        derivs[4] = solution[3];
        derivs[5] = solution[4];

        derivs[6] = (derivs[0] - derivs[3]) * Math.Exp(-P);
        derivs[7] = (derivs[1] - derivs[4]) * Math.Exp(-P);
        derivs[8] = (derivs[2] - derivs[5]) * Math.Exp(-P);
    }

    public override string ToString() => 
        string.Format("{0}: a = {1:F1}; b = {2:F1}", Name, a, b);
}
