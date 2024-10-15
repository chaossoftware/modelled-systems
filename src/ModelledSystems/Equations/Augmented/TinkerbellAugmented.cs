using ChaosSoft.Core;
using System;

namespace ModelledSystems.Equations.Augmented;

public sealed class TinkerbellAugmented : IAugmentedEquations, IHasName, IHasParameters
{
    private double a = 0.9;
    private double b = -0.6013;
    private double c = 2.0;
    private double d = 0.5;

    public TinkerbellAugmented()
    {
    }

    public int EqCount { get; } = 6;

    public string Name { get; } = "Tinkerbell Map Augmented";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
        d = parameters[3];
    }

    public double P { get; set; } = 0;

    public void F(double t, double[] solution, double[] derivs)
    {
        double x2 = solution[2];
        double y2 = solution[3];

        double x_ = x2 + solution[4];
        double y_ = y2 + solution[5];

        derivs[0] = x_ * x_ - y_ * y_ + a * x_ + b * y_;
        derivs[1] = 2 * x_ * y_ + c * x_ + d * y_;

        derivs[2] = x2 * x2 - y2 * y2 + a * x2 + b * y2;
        derivs[3] = 2 * x2 * y2 + c * x2 + d * y2;

        derivs[4] = (derivs[0] - derivs[2]) * Math.Exp(-P);
        derivs[5] = (derivs[1] - derivs[3]) * Math.Exp(-P);
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b", "c", "d"),
            a, b, c, d);
}
