using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations
{
    /// <summary>
    /// Hénon, M. & Heiles, C. [1964] “The applicability of the third integral of motion: some numerical experiments,” 
    /// The astronomical Journal 69, 73–79.
    /// </summary>
    public class HenonHeiles : SystemBase
    {
        protected const int EqCount = 4;

        private double x, vx, y, vy;

        /// <summary>
        /// Initializes a new instance of the <see cref="HenonHeiles"/> class.
        /// </summary>
        public HenonHeiles() : base(EqCount)
        {
        }

        public override string Name => "Hénon & Heiles system";

        public override void SetParameters(params double[] parameters)
        {
        }

        /// <summary>
        /// dx/dt = Vx<br/>
        /// dVx/dt = −x − 2xy<br/>
        /// dy/dt = Vy<br/>
        /// dVy/dt = −y − x² + y²<br/>
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

        public override string ToFileName() => "henon-heiles";
    }
}
