using ChaosSoft.NumericalMethods.Equations;

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
    /// Initializes a new instance of the <see cref="GeneralizedHenonMap"/> class 
    /// with default system parameters values:<br/>
    /// a = 1.9, b = 0.03.
    /// </summary>
    public GeneralizedHenonMap() : this(1.9, 0.03)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralizedHenonMap"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public GeneralizedHenonMap(double a, double b) : base(EqCount)
    {
        A = a;
        B = b;
    }

    public double A { get; private set; }

    public double B { get; private set; }

    public override string Name => "Generalized Hénon map";

    public override void SetParameters(params double[] parameters)
    {
        A = parameters[0];
        B = parameters[1];
    }

    /// <summary>
    /// Xₙ₊₁ = a − yₙ² − bzₙ<br/>
    /// yₙ₊₁ = xₙ<br/>
    /// zₙ₊₁ = yₙ
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
    /// [0, 0, 0].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        for (int i = 0; i < Count; i++)
        {
            current[0, i] = 0;
        }
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b"),
            A, B);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("gen-henon", "a", "b"),
            A, B);
}
