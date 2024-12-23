using ChaosSoft.Core;
using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations;

/// <summary>
/// Hénon, M. & Heiles, C. [1964] “The applicability of the third integral of motion: some numerical experiments,” 
/// The astronomical Journal 69, 73–79.
/// </summary>
public class HenonHeiles : IOdeSys, IHasFileName, IHasParameters, IHasName
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HenonHeiles"/> class.
    /// </summary>
    public HenonHeiles()
    {
    }

    public int EqCount { get; } = 4;

    public string Name { get; } = "Hénon & Heiles system";

    public void SetParameters(params double[] parameters)
    {
    }

    /// <summary>
    /// dx/dt = Vx<br/>
    /// dVx/dt = −x − 2xy<br/>
    /// dy/dt = Vy<br/>
    /// dVy/dt = −y − x² + y²<br/>
    /// </summary>
    /// <param name="current">current solution</param>
    /// <param name="derivs">derivatives</param>
    public void F(double t, double[] solution, double[] derivs)
    {
        double x = solution[0];
        double vx = solution[1];
        double y = solution[2];
        double vy = solution[3];

        derivs[0] = vx;
        derivs[1] = -x - 2 * x * y;
        derivs[2] = vy;
        derivs[3] = -y - x * x + y * y;
    }

    public override string ToString() => Name;

    public string ToFileName() => "henon-heiles";
}
