using MathLib.MathMethods.Solvers;
using System;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class AugmentedEquations : SystemEquations {

        public double p = 0;

        public AugmentedEquations() : base(false) { }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            throw new NotImplementedException();
        }


        public override void Init(double[,] x) {
            throw new NotImplementedException();
        }


        public override string GetInfoShort() {
            throw new NotImplementedException();
        }


        public override string GetInfoFull() {
            throw new NotImplementedException();
        }

        public override string ToFileName()
        {
            throw new NotImplementedException();
        }
    }
}
