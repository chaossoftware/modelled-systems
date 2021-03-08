using ChaosSoft.Core.NumericalMethods.Solvers;

namespace ModelledSystems
{
    /// <summary>
    /// Lorenz system equations
    /// parameters: sigma, r, b
    /// 3 linear and 9 non-linear equations
    /// Describes fluid flow
    /// </summary>
    public class Lorenz : SystemEquations
    {
        private readonly double _sg;
        private readonly double _r;
        private readonly double _b;

        public Lorenz() : this(false)
        {
        }

        public Lorenz(bool linearized) : this(0.01, linearized, 10.0, 28.0, 8.0 / 3.0)
        {
        }

        public Lorenz(double step, bool linearized, params double[] vars) : base(linearized)
        {
            _sg = vars[0];
            _r = vars[1];
            _b = vars[2];

            EquationsCount = 3;

            if (linearized)
            {
                TotalEquationsCount += EquationsCount;
            }

            Solver = new RK4(this, step);
        }

        public override string Name => "Lorenz";

        public override double[,] Derivatives(double[,] x, double[,] dxdt)
        {
            //Nonlinear Lorenz equations:
            dxdt[0, 0] = _sg * (x[0, 1] - x[0, 0]);
            dxdt[0, 1] = -x[0, 0] * x[0, 2] + _r * x[0, 0] - x[0, 1];
            dxdt[0, 2] = x[0, 0] * x[0, 1] - _b * x[0, 2];

            if (linearized)
            {
                //Linearized Lorenz equations:
                for (int i = 0; i < EquationsCount; i++)
                {
                    dxdt[1, i] = _sg * (x[2, i] - x[1, i]);
                    dxdt[2, i] = (_r - x[0, 2]) * x[1, i] - x[2, i] - x[0, 0] * x[3, i];
                    dxdt[3, i] = x[0, 1] * x[1, i] + x[0, 0] * x[2, i] - _b * x[3, i];
                }
            }

            return dxdt;
        }

        public override void Init(double[,] x)
        {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++)
            {
                x[0, i] = 1.0;

                if (linearized)
                {
                    x[i + 1, i] = 1.0;
                }
            }
        }

        public override string GetInfoShort() => Name;

        public override string GetInfoFull() =>
            string.Format("{0}: sigma = {1:F3}; rho = {2:F3}; b = {3:F3}; step size = {4:F3}", Name, _sg, _r, _b, Solver.Step);

        public override string ToFileName() =>
            string.Format("{0}_sigma={1:F1}_rho={2:F1}_b={3:F1}_st={4:F3}", Name, _sg, _r, _b, Solver.Step);
    }
}