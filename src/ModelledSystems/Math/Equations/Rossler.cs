using ChaosSoft.Core.NumericalMethods.Solvers;

namespace ModelledSystems
{
    /// <summary>
    /// Lorenz system equations
    /// parameters: a, b, c
    /// 3 non-linear and 9 linear equations
    /// Describes 
    /// </summary>
    public class Rossler : SystemEquations
    {

        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        public Rossler() : this(false)
        {
        }

        public Rossler(bool linearized) : this(0.1, linearized, 0.2, 0.2, 5.7)
        {
        }

        public Rossler(double step, bool linearized, params double[] vars) : base(linearized)
        {
            _a = vars[0];
            _b = vars[1];
            _c = vars[2];

            EquationsCount = 3;
            
            if (linearized)
            {
                TotalEquationsCount += EquationsCount;
            }

            Solver = new RK4(this, step);
        }

        public override string Name => "Rossler";

        public override double[,] Derivatives(double[,] x, double[,] dxdt)
        {
            //Nonlinear Rossler equations:
            dxdt[0, 0] = -x[0, 1] - x[0, 2];
            dxdt[0, 1] = x[0, 0] + _a * x[0, 1];
            dxdt[0, 2] = _b + x[0, 2] * (x[0, 0] - _c);

            if (linearized)
            {
                //Linearized Rossler equations:
                for (int i = 0; i < EquationsCount; i++)
                {
                    dxdt[1, i] = -x[2, i] - x[3, i];
                    dxdt[2, i] = x[1, i] + _a * x[2, i];
                    dxdt[3, i] = x[0, 2] * x[1, i] + x[0, 0] * x[3, i] - _c * x[3, i];
                }
            }

            return dxdt;
        }

        public override void Init(double[,] x)
        {
            //set diagonal and first n elements to 1
            for (int i = 0; i < EquationsCount; i++)
            {
                x[0, i] = 0.01;

                if (linearized)
                {
                    x[i + 1, i] = 0.01;
                }
            }
        }

        public override string GetInfoShort() => Name;

        public override string GetInfoFull() =>
            string.Format("{0}: a = {1:F1}; b = {2:F1}; c = {3:F1}; step size = {4:F3}", Name, _a, _b, _c, Solver.Step);

        public override string ToFileName() =>
            string.Format("{0}_a={1:F1}_b={2:F1}_c={3:F1}_st={4:F3}", Name, _a, _b, _c, Solver.Step);
    }
}