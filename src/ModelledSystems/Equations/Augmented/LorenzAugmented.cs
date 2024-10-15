using ChaosSoft.Core;
using System;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Lorenz system equations
/// 3 linear and 9 non-linear equations
/// Describes fluid flow
/// </summary>
public sealed class LorenzAugmented : IAugmentedEquations, IHasName, IHasParameters
{
    private double sg = 10.0;
    private double r = 28.0;
    private double b = 8.0 / 3.0;

    public LorenzAugmented()
    {
    }

    public int EqCount { get; } = 9;

    public string Name { get; } = "Lorenz Augmented";

    public double P { get; set; } = 0;

    public void SetParameters(params double[] parameters)
    {
        sg = parameters[0];
        r = parameters[1];
        b = parameters[2];
    }

    public void F(double t, double[] solution, double[] derivs)
    {
        double x00 = solution[3] + solution[6];
        double x01 = solution[4] + solution[7];
        double x02 = solution[5] + solution[8];

        //Nonlinear Lorenz equations:
        derivs[0] = sg * (x01 - x00);
        derivs[1] = -x00 * x02 + r * x00 - x01;
        derivs[2] = x00 * x01 - b * x02;

        derivs[3] = sg * (solution[4] - solution[3]);
        derivs[4] = -solution[3] * solution[5] + r * solution[3] - solution[4];
        derivs[5] = solution[3] * solution[4] - b * solution[5];

        derivs[6] = (derivs[0] - derivs[3]) * Math.Exp(-P);
        derivs[7] = (derivs[1] - derivs[4]) * Math.Exp(-P);
        derivs[8] = (derivs[2] - derivs[5]) * Math.Exp(-P);
    }

    public override string ToString() =>
        string.Format("{0}: sigma = {1:F3}; rho = {2:F3}; b = {3:F3}", Name, sg, r, b);
}