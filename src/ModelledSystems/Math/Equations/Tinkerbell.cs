using MathLib.MathMethods.Solvers;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class Tinkerbell : SystemEquations {

        private double a = 0.9;
        private double b = -0.6;
        private double c = 2.0;
        private double d = 0.5;

        public Tinkerbell(bool linearized = false) : base(linearized)
        {
            init();
        }

        public Tinkerbell(bool linearized = false, params double[] vars) : base(linearized)
        {
            a = vars[0];
            b = vars[1];
            c = vars[2];
            d = vars[3];
            init();
        }

        private void init() {
            EquationsCount = 2;
            if (linearized)
                TotalEquationsCount += EquationsCount;
            Solver = new SimpleSolver(this);
        }

        public override string Name => "Tinkerbell";

        public override double[,] Derivatives(double[,] x, double[,] dxdt) {


            //Nonlinear Tinkerbell map equations:
            dxdt[0, 0] = x[0, 0] * x[0, 0] - x[0, 1] * x[0, 1] + a * x[0, 0] + b * x[0, 1];
            dxdt[0, 1] = 2 * x[0, 0] * x[0, 1] + c * x[0, 0] + d * x[0, 1];

            if (linearized)
            {
                double x00Mul2 = 2 * x[0, 0];
                double x01Mul2 = 2 * x[0, 1];

                //Linearized Tinkerbell map equations:
                dxdt[1, 0] = x00Mul2 * x[1, 0] + a * x[1, 0] - x01Mul2 * x[2, 0] + b * x[2, 0]; //2 * x[0, 0] * x[1, 0] + a * x[1, 0] - 2 * x[0, 1] * x[2, 0] + b * x[2, 0]
                dxdt[1, 1] = x00Mul2 * x[1, 1] + a * x[1, 1] - x01Mul2 * x[2, 1] + b * x[2, 1]; //2 * x[0, 0] * x[1, 1] + a * x[1, 1] - 2 * x[0, 1] * x[2, 1] + b * x[2, 1]
                dxdt[2, 0] = x01Mul2 * x[1, 0] + c * x[1, 0] + x00Mul2 * x[2, 0] + d * x[2, 0]; //2 * x[0, 1] * x[1, 0] + c * x[1, 0] + 2 * x[0, 0] * x[2, 0] + d * x[2, 0]
                dxdt[2, 1] = x01Mul2 * x[1, 1] + c * x[1, 1] + x00Mul2 * x[2, 1] + d * x[2, 1]; //2 * x[0, 1] * x[1, 1] + c * x[1, 1] + 2 * x[0, 0] * x[2, 1] + d * x[2, 1]
            }

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++) {
                // for nonlinear maps, must be within the basin of attraction
                x[0, i] = 0.001;

                if (linearized)
                    //for linearized maps
                    x[i + 1, i] = 1.0; 
            }
        }


        public override string GetInfoShort() {
            return Name;
        }


        public override string GetInfoFull() {
            return string.Format("{0}: a = {1:F1}; b = {2:F1}; c = {3:F1}; d = {4:F1};step size = {5:F3}"
                , Name, a, b, c, d, Solver.Step);
        }

        public override string ToFileName()
        {
            return string.Format("{0}_a={1:F1}_b={2:F1}_c={3:F1}_d={4:F1}_st={5:F3}"
                , Name, a, b, c, d, Solver.Step);
        }
    }
}
