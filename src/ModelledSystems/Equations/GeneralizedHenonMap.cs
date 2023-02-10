using ChaosSoft.Core.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://www.sciencedirect.com/science/article/abs/pii/037596019090283T">Generalized Hénon map</see>.
/// </summary>
public class GeneralizedHenonMap : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;

    /// <summary>
    /// For a = 1.9, b = 0.03.
    /// </summary>
    public GeneralizedHenonMap() : this(1.9, 0.03)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: a, b)</param>
    public GeneralizedHenonMap(double a, double b) : base(EqCount)
    {
        A = a;
        B = b;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public override string Name => "Generalized Henon map";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
    }

    /// <summary>
    /// dx/dt = a — y² — bz<br/>
    /// dy/dt = x<br/>
    /// dz/dt = y
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = A - y * y - B * z;
        derivs[0, 1] = x;
        derivs[0, 2] = y;
    }

    /// <summary>
    /// Set all to 0.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0;
        }
    }

    public override string ToString() => $"{Name}: a = {A:F3}; b = {B:F4}";

    public override string ToFileName() => $"{Name}_a={A:F3}_b={B:F4}";
}
