using System;
using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// parameters: a, b
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class Henon : SystemEquations {

        private double a = 1.4;
        private double b = 0.3;

        public Henon(bool linearized = false) : base(linearized) {
            Init();
        }

        public Henon(bool linearized = false, params double[] vars) : base (linearized)
        {
            a = vars[0];
            b = vars[1];
            Init();
        }

        private void Init() {
            SystemName = "Henon Map";
            N = 2;
            if (Linearized)
                NN += N;
            Solver = new SimpleSolver(this);
        }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            //Nonlinear Henon map equations:
            dxdt[0, 0] = 1.0 - a * x[0, 0] * x[0, 0] + b * x[0, 1];
            dxdt[0, 1] = x[0, 0];

            if (Linearized) {
                double ax00 = a * x[0, 0]; // speed optimization

                //Linearized Henon map equations:
                dxdt[1, 0] = -2.0 * ax00 * x[1, 0] + b * x[2, 0]; //dxdt[1, 0] = -2.0 * a * x[0, 0] * x[1, 0] + b * x[2, 0];
                dxdt[1, 1] = -2.0 * ax00 * x[1, 1] + b * x[2, 1]; //dxdt[1, 1] = -2.0 * a * x[0, 0] * x[1, 1] + b * x[2, 1];
                dxdt[2, 0] = x[1, 0];
                dxdt[2, 1] = x[1, 1];

            }

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < N; i++) {
                x[0, i] = 0.0;

                if (Linearized)
                    x[i + 1, i] = 1.0;
            }
        }


        public override string GetInfoShort() {
            return SystemName;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: a = {1:F1}; b = {2:F1}; step size = {3:F3}"
                , SystemName, a, b, Solver.Step);
        }

        public override string ToFileName()
        {
            return string.Format("{0}_a={1:F1}_b={2:F1}_st={3:F3}"
                , SystemName, a, b, Solver.Step);
        }
    }
}
