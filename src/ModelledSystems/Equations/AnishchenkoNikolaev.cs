using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations
{
    /// <summary>
    /// Anishchenko, V. S. & Nikolaev, S. M. [2005] “Generator of quasi-periodic oscillations featuring 
    /// two-dimensional torus doubling bifurcations,” Technical Physics Letters 31, 853–855, doi:10.1134/1.2121837.
    /// </summary>
    public class AnishchenkoNikolaev : SystemBase
    {
        protected const int EqCount = 4;

        private double x, y, z, w;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnishchenkoNikolaev"/> class 
        /// with default system parameters values:<br/>
        /// α = 0.2, β = 0.43, δ = 0.001, μ = 0.0809
        /// </summary>
        public AnishchenkoNikolaev() : this(0.2, 0.43, 0.001, 0.0809)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnishchenkoNikolaev"/> class 
        /// with specific system parameters values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="d"></param>
        /// <param name="mu"></param>
        public AnishchenkoNikolaev(double a, double b, double d, double mu) : base(EqCount)
        {
            A = a;
            B = b;
            D = d;
            Mu = mu;
        }

        public double A { get; private set; }

        public double B { get; private set; }

        public double D { get; private set; }

        public double Mu { get; private set; }

        public override string Name => "Anishchenko & Nikolaev attractor";

        public override void SetParameters(params double[] parameters)
        {
            A = parameters[0];
            B = parameters[1];
            D = parameters[2];
            Mu = parameters[3];
        }

        /// <summary>
        /// dx/dt = −y<br/>
        /// dy/dt = x + μy − yw − δy³<br/>
        /// dz/dt = w<br/>
        /// dw/dt = −βz − αw + αФ(y)<para/>
        /// Ф(x) = I(x)x²<br/>
        /// I(x) = 1, x > 0; 0, x ≤ 0
        /// </summary>
        /// <param name="current">current solution</param>
        /// <param name="derivs">derivatives</param>
        public override void GetDerivatives(double[,] current, double[,] derivs)
        {
            x = current[0, 0];
            y = current[0, 1];
            z = current[0, 2];
            w = current[0, 3];

            derivs[0, 0] = -y;
            derivs[0, 1] = x + Mu * y - y * w - D * y * y * y;
            derivs[0, 2] = w;
            derivs[0, 3] = -B * z - A * w + A * F(y);
        }

        /// <summary>
        /// [1, 1, 1, 1].
        /// </summary>
        /// <param name="current">current solution</param>
        public override void SetInitialConditions(double[,] current)
        {
            for (int i = 0; i < Count; i++)
            {
                current[0, i] = 1;
            }
        }

        public override string ToString() =>
            string.Format(
                SysFormat.GetInfoTemplate(Name, "α", "β", "δ", "μ"),
                A, B, D, Mu);

        public override string ToFileName() =>
            string.Format(
                SysFormat.GetFileTemplate("anishchenko-nikolaev", "a", "b", "d", "mu"),
                A, B, D, Mu);

        private static double F(double x) =>
            x > 0 ? x * x : 0;
    }
}
