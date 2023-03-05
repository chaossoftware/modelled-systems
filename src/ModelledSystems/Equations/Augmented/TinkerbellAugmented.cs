using System;

namespace ModelledSystems.Equations.Augmented;

public class TinkerbellAugmented : AugmentedEquations
{
    protected const int EqCount = 6;

    private double a = 0.9;
    private double b = -0.6013;
    private double c = 2.0;
    private double d = 0.5;

    public TinkerbellAugmented() : base(EqCount)
    {
    }

    public TinkerbellAugmented(double a, double b, double c, double d) : base(EqCount)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    public override string Name => "Tinkerbell Map Augmented";

    public override void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
        d = parameters[3];
    }

    public override void GetDerivatives(double[,] current, double[,] derivs) 
    {
        double x2 = current[0, 2];
        double y2 = current[0, 3];

        double x_ = x2 + current[0, 4];
        double y_ = y2 + current[0, 5];

        derivs[0, 0] = x_ * x_ - y_ * y_ + a * x_ + b * y_;
        derivs[0, 1] = 2 * x_ * y_ + c * x_ + d * y_;

        derivs[0, 2] = x2 * x2 - y2 * y2 + a * x2 + b * y2;
        derivs[0, 3] = 2 * x2 * y2 + c * x2 + d * y2;

        derivs[0, 4] = (derivs[0, 0] - derivs[0, 2]) * Math.Exp(-p);
        derivs[0, 5] = (derivs[0, 1] - derivs[0, 3]) * Math.Exp(-p);
    }


    public override void SetInitialConditions(double[,] current) 
    {
        for (int i = 0; i < Count; i++) 
        {
            current[0, i] = 1e-8;
        }
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b", "c", "d"),
            a, b, c, d);

    public override string ToFileName() =>
        throw new NotImplementedException();
}
