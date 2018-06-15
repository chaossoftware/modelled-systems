using System;
using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class HenonGeneralizedAugmented : AugmentedEquations
    {

        private double a = 1.9;
        private double b = 0.03;

        public HenonGeneralizedAugmented()
        {
            init();
        }

        private void init() {
            EquationsCount = 9;
            Solver = new SimpleSolver(this);
        }

        public override string Name => "Generalized Henon Map Augmented";

        public override double[,] Derivatives(double[,] x, double[,] dxdt) {
            double x00 = x[0, 3] + x[0, 6];
            double x01 = x[0, 4] + x[0, 7];
            double x02 = x[0, 5] + x[0, 8];

            //Nonlinear Henon map equations:
            dxdt[0, 0] = a - x01 * x01 - b * x02;
            dxdt[0, 1] = x00;
            dxdt[0, 2] = x01;

            dxdt[0, 3] = a - x[0, 4] * x[0, 4] - b * x[0, 5];
            dxdt[0, 4] = x[0, 3];
            dxdt[0, 5] = x[0, 4];

            dxdt[0, 6] = (dxdt[0, 0] - dxdt[0, 3]) * Math.Exp(-p);
            dxdt[0, 7] = (dxdt[0, 1] - dxdt[0, 4]) * Math.Exp(-p);
            dxdt[0, 8] = (dxdt[0, 2] - dxdt[0, 5]) * Math.Exp(-p);

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++)
            {
                x[0, i] = 1e-8;
            }
        }


        public override string GetInfoShort() {
            return Name;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: a = {1:F1}; b = {2:F1}; step size = {3:F1}"
                , Name, a, b, Solver.Step);
        }

        public override string ToFileName()
        {
            return string.Format("{0}_a={1:F1}_b={2:F1}_st={3:F3}"
                , Name, a, b, Solver.Step);
        }
    }
}
