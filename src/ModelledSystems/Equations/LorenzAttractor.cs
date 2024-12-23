using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Equations system for 
/// <see href="https://en.wikipedia.org/wiki/Lorenz_system">Lorenz attractor</see>.
/// </summary>
public class LorenzAttractor : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double sg;
    protected double r;
    protected double b;

    /// <summary>
    /// Initializes a new instance of the <see cref="LorenzAttractor"/> class 
    /// with default system parameters values:<br/>
    /// ϛ = 10, r = 28, b = 8/3.
    /// </summary>
    public LorenzAttractor() : this(10, 28, 8d / 3d)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LorenzAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="sg"></param>
    /// <param name="r"></param>
    /// <param name="b"></param>
    public LorenzAttractor(double sg, double r, double b)
    {
        this.sg = sg;
        this.r = r;
        this.b = b;
    }

    public int EqCount { get; } = 3;

    public virtual string Name { get; } = "Lorenz system";

    public void SetParameters(params double[] parameters)
    {
        sg = parameters[0];
        r = parameters[1];
        b = parameters[2];
    }

    /// <summary>
    /// dx/dt = ϛ(y − x)<br/>
    /// dy/dt = x(r − z) − y<br/>
    /// dz/dt = xy − bz
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];

        derivs[0] = sg * (y - x);
        derivs[1] = x * (r - z) - y;
        derivs[2] = x * y - b * z;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "ϛ", "r", "b"),
            sg, r, b);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("lorenz", "sg", "r", "b"),
            sg, r, b);
}