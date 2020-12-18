using MathLib.NumericalMethods.Solvers;

namespace ModelledSystems
{
    /// <summary>
    /// Henon system equations
    /// parameters: a, b
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class Henon : SystemEquations
    {
        private readonly double _a;
        private readonly double _b;

        public Henon() : this(false)
        {
        }

        public Henon(bool linearized) : this(linearized, 1.4, 0.3)
        {
        }

        public Henon(bool linearized, params double[] vars) : base(linearized)
        {
            _a = vars[0];
            _b = vars[1];

            EquationsCount = 2;

            if (linearized)
            {
                TotalEquationsCount += EquationsCount;
            }

            Solver = new SimpleSolver(this);
        }

        public override string Name => "Henon Map";

        public override double[,] Derivatives(double[,] x, double[,] dxdt)
        {
            //Nonlinear Henon map equations:
            dxdt[0, 0] = 1.0 - _a * x[0, 0] * x[0, 0] + _b * x[0, 1];
            dxdt[0, 1] = x[0, 0];

            if (linearized)
            {
                double min2Mulax00 = -2.0 * _a * x[0, 0]; // speed optimization

                //Linearized Henon map equations:
                dxdt[1, 0] = min2Mulax00 * x[1, 0] + _b * x[2, 0]; //dxdt[1, 0] = -2.0 * a * x[0, 0] * x[1, 0] + b * x[2, 0];
                dxdt[1, 1] = min2Mulax00 * x[1, 1] + _b * x[2, 1]; //dxdt[1, 1] = -2.0 * a * x[0, 0] * x[1, 1] + b * x[2, 1];
                dxdt[2, 0] = x[1, 0];
                dxdt[2, 1] = x[1, 1];
            }

            return dxdt;
        }

        public override void Init(double[,] x)
        {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++)
            {
                x[0, i] = 0.0;

                if (linearized)
                {
                    x[i + 1, i] = 1.0;
                }
            }
        }

        public override string GetInfoShort() => Name;

        public override string GetInfoFull() =>
            string.Format("{0}: a = {1:F1}; b = {2:F1}; step size = {3:F3}", Name, _a, _b, Solver.Step);

        public override string ToFileName() =>
            string.Format("{0}_a={1:F1}_b={2:F1}_st={3:F3}", Name, _a, _b, Solver.Step);
    }
}
