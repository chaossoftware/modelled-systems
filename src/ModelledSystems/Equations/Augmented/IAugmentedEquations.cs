using ChaosSoft.NumericalMethods.Ode;

namespace ModelledSystems.Equations.Augmented;

/// <summary>
/// Henon system equations
/// 2 linear and 4 non-linear equations
/// </summary>
public interface IAugmentedEquations : IOdeSys
{
    double P { get; set; }

 }
