using System;
using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class HenonGeneralized : SystemEquations {

        private double a = 1.9;
        private double b = 0.03;

        public HenonGeneralized(bool linearized = false) : base(linearized)
        {
            init();
        }

        public HenonGeneralized(bool linearized = false, params double[] vars) : base(linearized)
        {
            a = vars[0];
            b = vars[1];
            init();
        }

        private void init()
        {
            EquationsCount = 3;
            if (linearized)
                TotalEquationsCount += EquationsCount;
            Solver = new SimpleSolver(this);
        }

        public override string Name => "Generalized Henon Map";

        public override double[,] Derivatives(double[,] x, double[,] dxdt) {

            //Nonlinear Henon map equations:
            //dxdt[0, 0] = 1 + x[0, 2] - a * x[0, 1] * x[0, 1];
            //dxdt[0, 1] = 1 + b * x[0, 1] - a * x[0, 0] * x[0, 0];
            //dxdt[0, 2] = b * x[0, 0];

            dxdt[0, 0] = a - x[0, 1] * x[0, 1] - b * x[0, 2];
            dxdt[0, 1] = x[0, 0];
            dxdt[0, 2] = x[0, 1];

            if (linearized)
            {
                for (int i = 0; i < EquationsCount; i++)
                { 
                    dxdt[1, i] = - 2 * x[0, 1] * x[2, i] - b * x[3, i];
                    dxdt[2, i] = x[1, i];
                    dxdt[3, i] = x[2, i];
                }


            }

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++) {
                x[0, i] = 0.0;

                if (linearized)
                    x[i + 1, i] = 1.0;
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
