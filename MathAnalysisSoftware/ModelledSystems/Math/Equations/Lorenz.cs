using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Lorenz system equations
    /// parameters: sigma, r, b
    /// 3 linear and 9 non-linear equations
    /// Describes fluid flow
    /// </summary>
    public class Lorenz : SystemEquations {

        private double sg = 10.0;
        private double r = 28.0;
        private double b = 8.0 / 3.0;

        public Lorenz(bool linearized = false) : base(linearized)
        {
            init(0.01);
        }

        public Lorenz(double step, bool linearized = false, params double[] vars) : base(linearized)
        {
            sg = vars[0];
            r = vars[1];
            b = vars[2];
            init(step);
        }

        private void init(double step) {
            this.SystemName = "Lorenz";
            N = 3;
            if (Linearized)
                NN += N;
            Solver = new RK4(this, step);
        }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {

            //Nonlinear Lorenz equations:
            dxdt[0, 0] = sg * (x[0, 1] - x[0, 0]);
            dxdt[0, 1] = -x[0, 0] * x[0, 2] + r * x[0, 0] - x[0, 1];
            dxdt[0, 2] = x[0, 0] * x[0, 1] - b * x[0, 2];

            if (Linearized)
            {
                //Linearized Lorenz equations:
                for (int i = 0; i < N; i++)
                {
                    dxdt[1, i] = sg * (x[2, i] - x[1, i]);
                    dxdt[2, i] = (r - x[0, 2]) * x[1, i] - x[2, i] - x[0, 0] * x[3, i];
                    dxdt[3, i] = x[0, 1] * x[1, i] + x[0, 0] * x[2, i] - b * x[3, i];
                }
            }

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < N; i++) {
                x[0, i] = 1.0;

                if (Linearized)
                    x[i + 1, i] = 1.0;
            }
        }


        public override string GetInfoShort() {
            return SystemName;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: sigma = {1:F3}; rho = {2:F3}; b = {3:F3}; step size = {4:F3}"
                , SystemName, sg, r, b, Solver.Step);
        }


        public override string ToFileName()
        {
            return string.Format("{0}_sigma={1:F1}_rho={2:F1}_b={3:F1}_st={4:F3}"
                , SystemName, sg, r, b, Solver.Step);
        }
    }
}