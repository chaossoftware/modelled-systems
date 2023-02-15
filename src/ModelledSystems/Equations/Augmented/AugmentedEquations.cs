using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations.Augmented;


/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public abstract class AugmentedEquations : SystemBase
{
    public double p = 0;

    protected AugmentedEquations(int equationsCount) : base(equationsCount) 
    { 
    }
}
