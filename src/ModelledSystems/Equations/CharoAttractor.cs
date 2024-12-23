using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;
using System;

namespace ModelledSystems.Equations;

/// <summary>
/// Char´o, G. D., Sciamarella, D., Mangiarotti, S., Artana, G. & Letellier, C. [2019] 
/// “Equivalence between the unsteady double-gyre system and a 4D autonomous conservative chaotic system,” 
/// Chaos 29, 123126, doi:10.1063/1.5120625.
/// </summary>
internal class CharoAttractor : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    private double a;
    private double nu;
    private double om;

    /// <summary>
    ///  Initializes a new instance of the <see cref="CharoAttractor"/> class 
    /// with default system parameters values:<br/>
    /// A = 0.1, η = 0.1, ω = π/5
    /// </summary>
    public CharoAttractor() : this(0.1, 0.1, Math.PI / 5)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CharoAttractor"/> class 
    /// with specific system parameters values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="nu"></param>
    /// <param name="om"></param>
    public CharoAttractor(double a, double nu, double om)
    {
        this.a = a;
        this.nu = nu;
        this.om = om;
    }

    public int EqCount { get; } = 4;

    public string Name { get; } = "Charó attractor";

    public void SetParameters(params double[] parameters)
    {
        a = parameters[0];
        nu = parameters[1];
        om = parameters[2];
    }

    /// <summary>
    /// dx/dt = Aπ sin(π [x²u + x − u]) sin(πy)<br/>
    /// dy/dt = Aπ cos(π [x²u + x − u]) cos(πy)<br/>
    /// du/dt = v
    /// dv/dt = −ω²u
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double y = solution[1];
        double u = solution[2];
        double v = solution[3];

        double aMulPi = a * Math.PI;
        double yMulPi = Math.PI * y;
        double expr = Math.PI * (x * x * u + x - u);

        derivs[0] = aMulPi * Math.Sin(expr) * Math.Sin(yMulPi);
        derivs[1] = aMulPi * Math.Cos(expr) * Math.Cos(yMulPi);
        derivs[2] = v;
        derivs[3] = -om * om * u;
    }


    public override string ToString() =>
        string.Format(
            SysFormat.GetInfoTemplate(Name, "A", "η", "ω"),
            a, nu, om);

    public string ToFileName() =>
        string.Format(
            SysFormat.GetFileTemplate("charo", "a", "nu", "om"),
            a, nu, om);
}
