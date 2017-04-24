using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// parameters: a, b
    /// 2 linear and 4 non-linear equations
    /// </summary>
    class Logistic : SystemEquations {

        private double r = 4;

        public Logistic(bool linearized = false) : base(linearized)
        {
            Init();
        }

        public Logistic(bool linearized = false, params double[] vars) : base(linearized)
        {
            r = vars[0];
            Init();
        }


        private void Init()
        {
            SystemName = "Logistic Map";
            N = 1;
            if (Linearized)
                NN += N;

            Solver = new SimpleSolver(this);
        }

        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            //Nonlinear Logistic map equations:
            dxdt[0, 0] = r * x[0, 0] * (1 - x[0, 0]);

            if (Linearized)
                //Linearized Logistic map equations:
                dxdt[1, 0] = r - 2 * r * x[0, 0];

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < N; i++) {
                x[0, i] = 0.1;

                if (Linearized)
                    x[i + 1, i] = 1;
            }
        }


        public override string GetInfoShort() {
            return SystemName;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: r = {1:F1}; step size = {2:F3}"
                , SystemName, r, Solver.Step);
        }


        public override string ToFileName()
        {
            return string.Format("{0}_r={1:F1}_st={2:F3}"
                , SystemName, r, Solver.Step);
        }
    }
}
