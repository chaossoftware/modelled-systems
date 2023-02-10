using ChaosSoft.Core.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/f3263f7618879.560aea484fce0.jpg">
/// Qi-Chen attractor</see>.<br/>
/// (<see href="https://www.behance.net/gallery/7618879/Strange-Attractors">All attractors</see>)
/// </summary>
public class QiChenAttractor : SystemBase
{
    protected const int EqCount = 3;

    private double x, y, z;

    /// <summary>
    /// For α = 38, β = 8/3, ς = 80.
    /// </summary>
    public QiChenAttractor() : this(38, 8/3, 80)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vars">params array (order: α, β, ς)</param>
    public QiChenAttractor(double a, double b, double c) : base(EqCount)
    {
        A = a;
        B = b;
        C = c;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public double C { get; private set; }

    public override string Name => "Qi-Chen attractor";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
        C = parameters[2];
    }

    /// <summary>
    /// dx/dt = α(y — x) + yz<br/>
    /// dy/dt = ςx — y — xz<br/>
    /// dz/dt = xy — βz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];
        y = current[0, 1];
        z = current[0, 2];

        derivs[0, 0] = A * (y - x) + y * z;
        derivs[0, 1] = C * x - y - x * z;
        derivs[0, 2] = x * y - B * z;
    }

    /// <summary>
    /// Set all to 1.
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 1;
        }
    }

    public override string ToString() => $"{Name}: b = {B:F2}";

    public override string ToFileName() => $"{Name}_b={B:F2}";
}
