using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Logistic_map">Logistic map</see>.
/// </summary>
public class LogisticMap : SystemBase
{
    protected const int EqCount = 1;

    private double x;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogisticMap"/> class 
    /// with default system parameters values:<br/>
    /// R = 4.
    /// </summary>
    public LogisticMap() : this(4)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogisticMap"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="r"></param>
    public LogisticMap(double r) : base(EqCount)
    {
        R = r;
    }

    public double R { get; private set; }

    public override string Name => "Logistic map";

    public override void SetParameters(params double[] parameters)
    {
        R = parameters[0];
    }

    /// <summary>
    /// xₙ₊₁ = Rxₙ * (1 − xₙ)
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public override void GetDerivatives(double[,] current, double[,] derivs)
    {
        x = current[0, 0];

        derivs[0, 0] = R * x * (1 - x);
    }

    /// <summary>
    /// [0.1].
    /// </summary>
    /// <param name="current">current solution</param>
    public override void SetInitialConditions(double[,] current)
    {
        current[0, 0] = 0.1;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "R"),
            R);

    public override string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("logistic", "R"),
            R);
}
