using System;
using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Ikeda system
    /// </summary>
    public class Ikeda : SystemEquations {

        private double a = 20.0;
        private double b = 1.0;
        private double c = Math.Atan(1.0);

        public Ikeda(bool linearized = false) : base(linearized)
        {
            init();
        }

        public Ikeda(bool linearized = false, params double[] vars) : base(linearized)
        {
            a = vars[0];
            b = vars[1];
            c = vars[2];
            init();

        }

        private void init() {
            this.SystemName = "Ikeda";
            N = 101;
            if (Linearized)
                NN += N;

            Solver = new SimpleSolver(this);
            Solver.Step = 8d * Math.Atan(1d) / (N - 1);
        }

        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            //Ikeda Equations
            dxdt[0, 0] = x[0, 0] + Solver.Step * (a * Math.Sin(x[0, N - 1] - c) * Math.Sin(x[0, N - 1] - c) - b * x[0, 0]);

            for (int i = 0; i < N - 1; i++)
                dxdt[0, i + 1] = x[0, i];

            if (Linearized)
            {

                //Linearized Equations
                for (int i = 0; i < N; i++)
                    dxdt[1, i] = x[1, i] + Solver.Step * (x[N, i] * 2.0 * a * Math.Sin(x[0, N - 1] - c) * Math.Cos(x[0, N - 1] - c) - b * x[1, i]);

                for (int i = 2; i <= N; i++)
                    for (int j = 0; j < N; j++)
                        dxdt[i, j] = x[i - 1, j];
            }

            return dxdt;
        }


        public override void Init(double[,] x) {
            for (int i = 0; i < N; i++) {
                x[0, i] = 0.9;

                if (Linearized)
                    x[i + 1, i] = 1.0;
            }
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
