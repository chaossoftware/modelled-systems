using System;
using System.Drawing;
using MathLib.MathMethods.Solvers;

namespace ModelledSystems.Routines
{
    abstract class Routine
    {
        protected Routine(string outDir, SystemParameters parameters)
        {
            OutDir = outDir;
            SysParameters = parameters;
        }

        protected string OutDir { get; set; }

        protected SystemParameters SysParameters { get; set; }

        public Size Size { get; set; }

        public abstract void Run();

        protected SystemEquations GetSystemEquations(bool linearized, double[] vars, double step)
        {
            SystemEquations eq;
            switch (SysParameters.SystemName.ToLower())
            {
                case "lorenz":
                    eq = new Lorenz(step, linearized, vars);
                    break;
                case "rossler":
                    eq = new Rossler(step, linearized, vars);
                    break;
                case "ikeda":
                    eq = new Ikeda(linearized, vars);
                    break;
                case "henon":
                    eq = new Henon(linearized, vars);
                    break;
                case "henon_generalized":
                    eq = new HenonGeneralized(linearized, vars);
                    break;
                case "logistic":
                    eq = new Logistic(linearized, vars);
                    break;
                case "tinkerbell":
                    eq = new Tinkerbell(linearized, vars);
                    break;
                default:
                    throw new ArgumentException($"No such system: {SysParameters.SystemName}");
            }

            eq.Solver.Step = step;
            return eq;
        }
    }
}
