using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Klein, M. & Baier, G. [1991] “Hierarchies of dynamical systems,” A chaotic hierarchy 
/// (World Scientific Publishing), pp. 1–24.
/// </summary>
public class KleinBaier : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    protected double a;
    protected double b;
    protected double c;
    protected double d;
    protected double e;

    /// <summary>
    /// Initializes a new instance of the <see cref="KleinBaier"/> class 
    /// with default system parameters values:<br/>
    /// a = 0.15, b = 0.25, c = 0.01, d = 0.3922, e = 0.05
    /// </summary>
    public KleinBaier() : this(0.15, 0.25, 0.01, 0.3922, 0.05)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KleinBaier"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="e"></param>
    public KleinBaier(double a, double b, double c, double d, double e)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.e = e;
    }

    public int EqCount { get; } = 4;

    public string Name { get; } = "Klein & Baier";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        b = parameters[1];
        c = parameters[2];
        d = parameters[3];
        e = parameters[4];
    }

    /// <summary>
    /// dx/dt = −y - az - bw<br/>
    /// dy/dt = x<br/>
    /// dz/dt = −cw + d - dy²<br/>
    /// dw/dt = cz - ew<para/>
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double z = solution[2];
        double w = solution[3];

        derivs[0] = -y - a * z - b * w;
        derivs[1] = x;
        derivs[2] = -c * w + d - d * y * y;
        derivs[3] = c * z - e * w;
    }

    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "a", "b", "c", "d", "e"),
            a, b, c, d, e);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("klein-baier", "a", "b", "c", "d", "e"),
            a, b, c, d, e);
}
