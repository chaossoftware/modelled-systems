using MathLib.MathMethods.Solvers;
using System;

namespace ModelledSystems
{

    /// <summary>
    /// Lorenz system equations
    /// 3 non-linear and 9 linear equations
    /// Describes 
    /// </summary>
    public class RosslerAugmented : AugmentedEquations {

        private double a = 0.2;
        private double b = 0.2;
        private double c = 5.7;

        public RosslerAugmented() {
            init();
        }

        public RosslerAugmented(double a, double b, double c) {
            this.a = a;
            this.b = b;
            this.c = c;
            init();
        }

        private void init() {
            EquationsCount = 9;
            Solver = new RK4(this, 0.01);
        }

        public override string Name => "Rossler Augmented";

        public override double[,] Derivatives(double[,] x, double[,] dxdt) {

            double x00 = x[0, 3] + x[0, 6];
            double x01 = x[0, 4] + x[0, 7];
            double x02 = x[0, 5] + x[0, 8];

            //Nonlinear Rossler equations:
            dxdt[0, 0] = -x01 - x02;
            dxdt[0, 1] = x00 + a * x01;
            dxdt[0, 2] = b + x02 * (x00 - c);

            dxdt[0, 3] = -x[0, 4] - x[0, 5];
            dxdt[0, 4] = x[0, 3] + a * x[0, 4];
            dxdt[0, 5] = b + x[0, 5] * (x[0, 3] - c);

            dxdt[0, 6] = (dxdt[0, 0] - dxdt[0, 3]) * Math.Exp(-p);
            dxdt[0, 7] = (dxdt[0, 1] - dxdt[0, 4]) * Math.Exp(-p);
            dxdt[0, 8] = (dxdt[0, 2] - dxdt[0, 5]) * Math.Exp(-p);

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++) {
                x[0, i] = 1e-8;
            }
        }


        public override string GetInfoShort() {
            return "Rossler";
        }


        public override string GetInfoFull() {
            throw new System.NotImplementedException();
        }

        public override string ToFileName()
        {
            throw new NotImplementedException();
        }
    }
}