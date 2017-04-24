using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Lorenz system equations
    /// parameters: a, b, c
    /// 3 non-linear and 9 linear equations
    /// Describes 
    /// </summary>
    public class Rossler : SystemEquations {

        private double a = 0.2;
        private double b = 0.2;
        private double c = 5.7;

        public Rossler(bool linearized = false) : base(linearized)
        {
            init(0.1);
        }

        public Rossler(double step, bool linearized = false, params double[] vars) : base(linearized)
        {
            a = vars[0];
            b = vars[1];
            c = vars[2];
            init(step);
        }

        private void init(double step) {
            this.SystemName = "Rossler";
            N = 3;
            if (Linearized)
                NN += N;
            Solver = new RK4(this, step);
        }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            //Nonlinear Rossler equations:
            dxdt[0, 0] = -x[0, 1] - x[0, 2];
            dxdt[0, 1] = x[0, 0] + a * x[0, 1];
            dxdt[0, 2] = b + x[0, 2] * (x[0, 0] - c);

            if (Linearized)
            {
                //Linearized Rossler equations:
                for (int i = 0; i < N; i++)
                {
                    dxdt[1, i] = -x[2, i] - x[3, i];
                    dxdt[2, i] = x[1, i] + a * x[2, i];
                    dxdt[3, i] = x[0, 2] * x[1, i] + x[0, 0] * x[3, i] - c * x[3, i];
                }
            }

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < N; i++) {
                x[0, i] = 0.01;

                if (Linearized)
                    x[i + 1, i] = 0.01;
            }
        }


        public override string GetInfoShort() {
            return SystemName;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: a = {1:F1}; b = {2:F1}; c = {3:F1}; step size = {4:F3}"
                , SystemName, a, b, c, Solver.Step);
        }


        public override string ToFileName()
        {
            return string.Format("{0}_a={1:F1}_b={2:F1}_c={3:F1}_st={4:F3}"
                , SystemName, a, b, c, Solver.Step);
        }
    }
}