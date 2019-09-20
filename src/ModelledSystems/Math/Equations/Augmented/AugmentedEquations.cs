using MathLib.NumericalMethods.Solvers;
using System;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public abstract class AugmentedEquations : SystemEquations
    {

        public double p = 0;

        public AugmentedEquations() : base(false) { }

    }
}
