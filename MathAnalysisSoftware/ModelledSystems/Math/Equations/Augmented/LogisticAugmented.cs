﻿using MathLib.MathMethods.Solvers;
using System;

namespace ModelledSystems
{

    /// <summary>
    /// Henon system equations
    /// 2 linear and 4 non-linear equations
    /// </summary>
    public class LogisticAugmented : AugmentedEquations
    {
        private double a = 4;

        public LogisticAugmented() {
            this.SystemName = "Logistic Map";
            N = 3;
            Solver = new SimpleSolver(this);
        }

        public LogisticAugmented(double a)
        {
            this.a = a;
            this.SystemName = "Logistic Map";
            N = 3;
            Solver = new SimpleSolver(this);
        }


        public override double[,] Derivs(double[,] x, double[,] dxdt) {
            double x00 = x[0, 1] + x[0, 2];

            //Nonlinear Logistic map equations:
            dxdt[0, 0] = a * x00 - a * Math.Pow(x00, 2);
            dxdt[0, 1] = a * x[0, 1] * (1 - x[0, 1]);
            dxdt[0, 2] = (dxdt[0, 0] - dxdt[0, 1]) * Math.Exp(-p);

            return dxdt;
        }


        public override void Init(double[,] x) {
            //set diagonal and first n elements to 1
            for (int i = 0; i < N; i++) {
                x[0, i] = 1e-8;
            }
        }


        public override string GetInfoShort() {
            return "Logistic Map";
        }


        public override string GetInfoFull() {
            throw new NotImplementedException();
        }
    }
}