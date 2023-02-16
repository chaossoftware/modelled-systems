using ChaosSoft.NumericalMethods.Equations;
using System;

namespace ModelledSystems.Equations
{
    /// <summary>
    /// Char´o, G. D., Sciamarella, D., Mangiarotti, S., Artana, G. & Letellier, C. [2019] 
    /// “Equivalence between the unsteady double-gyre system and a 4D autonomous conservative chaotic system,” 
    /// Chaos 29, 123126, doi:10.1063/1.5120625.
    /// </summary>
    internal class CharoAttractor : SystemBase
    {
        protected const int EqCount = 4;

        private double x, y, u, v, aMulPi, yMulPi, expr;

        /// <summary>
        ///  Initializes a new instance of the <see cref="CharoAttractor"/> class 
        /// with default system parameters values:<br/>
        /// A = 0.1, η = 0.1, ω = π/5
        /// </summary>
        public CharoAttractor() : this(0.1, 0.1, Math.PI / 5)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharoAttractor"/> class 
        /// with specific system parameters values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="nu"></param>
        /// <param name="om"></param>
        public CharoAttractor(double a, double nu, double om) : base(EqCount)
        {
            A = a;
            Nu = nu;
            Om = om;
        }

        public double A { get; private set; }

        public double Nu { get; private set; }

        public double Om { get; private set; }

        public override string Name => "Charó attractor";

        public override void SetParameters(params double[] parameters)
        {
            A = parameters[0];
            Nu = parameters[1];
            Om = parameters[2];
        }

        /// <summary>
        /// dx/dt = Aπ sin(π [x²u + x − u]) sin(πy)<br/>
        /// dy/dt = Aπ cos(π [x²u + x − u]) cos(πy)<br/>
        /// du/dt = v
        /// dv/dt = −ω²u
        /// </summary>
        /// <param name="current">current solution</param>
        /// <param name="derivs">derivatives</param>
        public override void GetDerivatives(double[,] current, double[,] derivs)
        {
            x = current[0, 0];
            y = current[0, 1];
            u = current[0, 2];
            v = current[0, 3];

            aMulPi = A * Math.PI;
            yMulPi = Math.PI * y;
            expr = Math.PI * (x * x * u + x - u);

            derivs[0, 0] = aMulPi * Math.Sin(expr) * Math.Sin(yMulPi);
            derivs[0, 1] = aMulPi * Math.Cos(expr) * Math.Cos(yMulPi);
            derivs[0, 2] = v;
            derivs[0, 3] = -Om * Om * u;
        }

        /// <summary>
        /// [0.2, 0, 0, ωη].
        /// </summary>
        /// <param name="current">current solution</param>
        public override void SetInitialConditions(double[,] current)
        {
            current[0, 0] = 0.2;
            current[0, 1] = 0;
            current[0, 2] = 0;
            current[0, 3] = Om * Nu;
        }

        public override string ToString() =>
            string.Format(
                SysFormat.GetInfoTemplate(Name, "A", "η", "ω"),
                A, Nu, Om);

        public override string ToFileName() =>
            string.Format(
                SysFormat.GetFileTemplate("charo", "a", "nu", "om"),
                A, Nu, Om);
    }
}
