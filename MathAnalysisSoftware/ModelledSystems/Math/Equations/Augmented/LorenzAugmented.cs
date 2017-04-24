using MathLib.MathMethods.Solvers;
using System;

namespace ModelledSystems
{

    /// <summary>
    /// Lorenz system equations
    /// 3 linear and 9 non-linear equations
    /// Describes fluid flow
    /// </summary>
    public class LorenzAugmented : AugmentedEquations
    {

        private double sg = 10.0;
        private double r = 28.0;
        private double b = 8.0 / 3.0;

        public LorenzAugmented() {
            init();
        }

        public LorenzAugmented(double sigma, double r, double b) {
            this.sg = sigma;
            this.r = r;
            this.b = b;
            init();
        }

        private void init() {
            this.SystemName = "Lorenz";
            N = 9;
            Solver = new RK4(this, 0.01);
        }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            double x00 = x[0, 3] + x[0, 6];
            double x01 = x[0, 4] + x[0, 7];
            double x02 = x[0, 5] + x[0, 8];

            //Nonlinear Lorenz equations:
            dxdt[0, 0] = sg * (x01 - x00);
            dxdt[0, 1] = -x00 * x02 + r * x00 - x01;
            dxdt[0, 2] = x00 * x01 - b * x02;

            dxdt[0, 3] = sg * (x[0, 4] - x[0, 3]);
            dxdt[0, 4] = -x[0, 3] * x[0, 5] + r * x[0, 3] - x[0, 4];
            dxdt[0, 5] = x[0, 3] * x[0, 4] - b * x[0, 5];

            dxdt[0, 6] = (dxdt[0, 0] - dxdt[0, 3]) * Math.Exp(-p);
            dxdt[0, 7] = (dxdt[0, 1] - dxdt[0, 4]) * Math.Exp(-p);
            dxdt[0, 8] = (dxdt[0, 2] - dxdt[0, 5]) * Math.Exp(-p);

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
            return string.Format("{0}: sigma = {1:F3}; rho = {2:F3}; b = {3:F3}; step size = {4:F3}"
                , SystemName, sg, r, b, Solver.Step);
        }
    }
}