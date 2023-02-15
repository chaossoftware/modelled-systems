using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations
{
    public class HenonHeiles : SystemBase
    {
        protected const int EqCount = 4;

        private double x, vx, y, vy;

        public HenonHeiles() : base(EqCount)
        {
        }

        public override string Name => "Henon & Heiles";

        public override void SetParameters(params double[] parameters)
        {
        }

        /// <summary>
        /// dx/dt = ϛ(y — x)<br/>
        /// dy/dt = x(ρ — z) — y<br/>
        /// dz/dt = xy — βz
        /// </summary>
        /// <param name="current">current solution</param>
        /// <param name="derivs">derivatives</param>
        public override void GetDerivatives(double[,] current, double[,] derivs)
        {
            x = current[0, 0];
            vx = current[0, 1];
            y = current[0, 2];
            vy = current[0, 3];

            derivs[0, 0] = vx;
            derivs[0, 1] = -x - 2 * x * y;
            derivs[0, 2] = vy;
            derivs[0, 3] = -y - x * x + y * y;
        }

        /// <summary>
        /// [0, 0.478, −0.15, 0].
        /// </summary>
        /// <param name="current">current solution</param>
        public override void SetInitialConditions(double[,] current)
        {
            current[0, 0] = 0;
            current[0, 1] = 0.478;
            current[0, 2] = -0.15;
            current[0, 3] = 0;
        }

        public override string ToString() => Name;

        public override string ToFileName() => Name;
    }
}
