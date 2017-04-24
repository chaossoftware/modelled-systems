using MathLib.MathMethods.Solvers;
using System;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class HenonAugmented : AugmentedEquations
    {

        private double a = 1.4;
        private double b = 0.3;

        public HenonAugmented() {
            init();
        }

        public HenonAugmented(double a, double b)
        {
            this.a = a;
            this.b = b;
            init();
        }

        private void init() {
            this.SystemName = "Henon Map";
            N = 6;
            Solver = new SimpleSolver(this);
        }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            double x00 = x[0, 2] + x[0, 4];
            double x01 = x[0, 3] + x[0, 5];

            //Nonlinear Henon map equations:
            dxdt[0, 0] = 1.0 - a * Math.Pow(x00, 2) + x01;
            dxdt[0, 1] = b * x00;
            dxdt[0, 2] = 1 - a * Math.Pow(x[0, 2], 2) + x[0, 3];
            dxdt[0, 3] = b * x[0, 2];
            dxdt[0, 4] = (dxdt[0, 0] - dxdt[0, 2]) * Math.Exp(-p);
            dxdt[0, 5] = (dxdt[0, 1] - dxdt[0, 3]) * Math.Exp(-p);

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < N; i++) {
                x[0, i] = 1e-8;
            }
        }


        public override string GetInfoShort() {
            return SystemName;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: a = {1:F1}; b = {2:F1}; step size = {3:F1}"
                , SystemName, a, b, Solver.Step);
        }
    }
}
