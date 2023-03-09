using ChaosSoft.NumericalMethods.Equations;

namespace ModelledSystems.Equations
{
    /// <summary>
    /// Klein, M. & Baier, G. [1991] “Hierarchies of dynamical systems,” A chaotic hierarchy 
    /// (World Scientific Publishing), pp. 1–24.
    /// </summary>
    public class KleinBaier : SystemBase
    {
        protected const int EqCount = 4;

        private double x, y, z, w;

        /// <summary>
        /// Initializes a new instance of the <see cref="KleinBaier"/> class 
        /// with default system parameters values:<br/>
        /// a = 0.15, b = 0.25, c = 0.01, d = 0.3922, e = 0.05
        /// </summary>
        public KleinBaier() : this(0.15, 0.25, 0.01, 0.3922, 0.05)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KleinBaier"/> class 
        /// with specific system parameters values.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public KleinBaier(double a, double b, double c, double d, double e) : base(EqCount)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
        }

        public double A { get; private set; }

        public double B { get; private set; }

        public double C { get; private set; }

        public double D { get; private set; }

        public double E { get; private set; }

        public override string Name => "Klein & Baier";

        public override void SetParameters(params double[] parameters)
        {
            A = parameters[0];
            B = parameters[1];
            C = parameters[2];
            D = parameters[3];
            E = parameters[4];
        }

        /// <summary>
        /// dx/dt = −y - az - bw<br/>
        /// dy/dt = x<br/>
        /// dz/dt = −cw + d - dy²<br/>
        /// dw/dt = cz - ew<para/>
        /// </summary>
        /// <param name="current">current solution</param>
        /// <param name="derivs">derivatives</param>
        public override void GetDerivatives(double[,] current, double[,] derivs)
        {
            x = current[0, 0];
            y = current[0, 1];
            z = current[0, 2];
            w = current[0, 3];

            derivs[0, 0] = -y - A * z - B * w;
            derivs[0, 1] = x;
            derivs[0, 2] = -C * w + D - D * y * y;
            derivs[0, 3] = C * z - E * w;
        }

        /// <summary>
        /// [-0.157, -0.043, 3.113, 1.826].
        /// </summary>
        /// <param name="current">current solution</param>
        public override void SetInitialConditions(double[,] current)
        {
            current[0, 0] = -0.157;
            current[0, 1] = -0.043;
            current[0, 2] = 3.113;
            current[0, 3] = 1.826;
        }

        public override string ToString() =>
            string.Format(
                SysFormat.GetInfoTemplate(Name, "a", "b", "c", "d", "e"),
                A, B, C, D, E);

        public override string ToFileName() =>
            string.Format(
                SysFormat.GetFileTemplate("klein-baier", "a", "b", "c", "d", "e"),
                A, B, C, D, E);
    }
}
