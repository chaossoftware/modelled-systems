using ChaosSoft.NumericalMethods.Equations;
using System;

namespace ModelledSystems.Equations
{
    public class AnishchenkoNikolaev : SystemBase
    {
        protected const int EqCount = 4;

        private double x, y, z, w;

        /// <summary>
        /// α = 0.2, β = 0.43, δ = 0.001, μ = 0.0809, θ = pi/3
        /// </summary>
        public AnishchenkoNikolaev() : this(0.2, 0.43, 0.001, 0.0809, Math.PI / 3)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vars">params array (order: a)</param>
        public AnishchenkoNikolaev(double a, double b, double d, double mu, double th) : base(EqCount)
        {
            A = a;
            B = b;
            D = d;
            Mu = mu;
            Th = th;
        }

        public double A { get; private set; }

        public double B { get; private set; }

        public double D { get; private set; }

        public double Mu { get; private set; }

        public double Th { get; private set; }

        public override string Name => "Anishchenko & Nikolaev";

        public override void SetParameters(params double[] parameters)
        {
            A = parameters[0];
            B = parameters[1];
            D = parameters[2];
            Mu = parameters[3];
            Th = parameters[4];
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
            current[0, 0] = 1;
            current[0, 1] = 1;
            current[0, 2] = 1;
            current[0, 3] = 1;
        }

        public override string ToString() =>
            string.Format(GetInfoTemplate("α", "β", "δ", "μ", "θ"),
            A, B, D, Mu, Th);

        public override string ToFileName() =>
            string.Format(GetFileNameTemplate("a", "b", "d", "mu", "th"),
            A, B, D, Mu, Th);

        private double F(double x) =>
            x > 0 ? x * x : 0;
    }
}
