using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using MathLib.DrawEngine.Charts;
using MathLib.MathMethods.Lyapunov;
using MathLib.MathMethods.Orthogonalization;
using MathLib.MathMethods.Solvers;
using MathLib.Threading;

namespace ModelledSystems.Routines
{
    internal class LyapunovExponentsMap : Routine
    {
        private readonly double xBegin, yBegin, xStep, yStep, xEnd, yEnd;
        private readonly int xParamIndex, yParamIndex;
        private readonly double[,] arr;

        private readonly int xIterations, yIterations;
        private readonly double eqStep;

        private readonly int step;
        private int currentIteration;
        private readonly Parameter xParameter, yParameter;

        public LyapunovExponentsMap(string outDir, SystemParameters systemParameters, int xParamIndex, int yParamIndex) : base(outDir, systemParameters)
        {
            this.xParamIndex = xParamIndex;
            this.yParamIndex = yParamIndex;
            eqStep = SysParameters.Step.Default;

            xParameter = SysParameters.ListParameters[xParamIndex];
            yParameter = SysParameters.ListParameters[yParamIndex];

            xBegin = xParameter.Start;
            yBegin = yParameter.Start;
            xStep = xParameter.Step;
            yStep = yParameter.Step;
            xEnd = xParameter.End;
            yEnd = yParameter.End;

            xIterations = (int)((xEnd - xBegin) / xStep);
            yIterations = (int)((yEnd - yBegin) / yStep);

            currentIteration = 1;
            var totalIterations = xIterations * yIterations;

            step = totalIterations / Console.BufferWidth;

            arr = new double[xIterations, yIterations];

            for (int i = 0; i < xIterations; i++)
                for (int j = 0; j < yIterations; j++)
                    arr[i, j] = -1;
        }

        public override void Run()
        {
            ThreadedRun threadedRun = new ThreadedRun();
            double yVal;
            double xVal = xBegin;

            for (int x = 0; x < xIterations; x++)
            {
                xVal += xStep;
                yVal = yBegin;

                for (int y = 0; y < yIterations; y++)
                {
                    yVal += yStep;
                    threadedRun.RunOnSeparateProcessor(Func, new object[4] { xVal, yVal, x, y });

                    if (currentIteration++ % step == 0)
                        Console.Write("#");
                }
            }

            threadedRun.WaitForAllTasks();

            GetImage();
        }

        private void GetImage()
        {
            ColouredMapPlot po = new ColouredMapPlot(arr, Size, new ColorCondition4());
            po.Xmin = xBegin;
            po.Xmax = xEnd;
            po.Ymin = yBegin;
            po.Ymax = yEnd;
            po.LabelX = xParameter.Name;
            po.LabelY = yParameter.Name;
            po.Plot().Save(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_map.png"), ImageFormat.Png);
        }

        public void Func(object[] parameters)
        {
            double xparam = (double)parameters[0], yParam = (double)parameters[1];
            int x = (int)parameters[2], y = (int)parameters[3];

            int rez = 0;
            long totIter;
            double[] R, vars;

            vars = SysParameters.Defaults;
            vars[xParamIndex] = xparam;
            vars[yParamIndex] = yParam;

            SystemEquations equations = GetSystemEquations(true, vars, eqStep);
            totIter = (long)(SysParameters.ModellingTime / equations.Solver.Step);
            Orthogonalization ort = new ModifiedGrammSchmidt(equations.EquationsCount);
            BenettinMethod lyap = new BenettinMethod(equations.EquationsCount);

            R = new double[equations.EquationsCount];
            equations.Solver.Init();

            for (int i = 0; i < totIter; i++)
            {
                equations.Solver.NexStep();
                ort.Perform(equations.Solver.Solution, R);
                lyap.calculateLE(R, equations.Solver.Time);
                totIter--;
            }

            for (int k = 0; k < equations.EquationsCount; k++)
                if (lyap.lespec[k] > 0)
                    rez++;

            arr[x, y] = rez;
        }
    }

    class ColorCondition4 : MathLib.DrawEngine.Charts.ColorMaps.ColorMap
    {
        public Color GetColor(double value)
        {
            switch ((int)value)
            {
                case 3:
                    return Color.Black;
                case 2:
                    return Color.DarkGreen;
                case 1:
                    return Color.Green;
                case 0:
                    return Color.Blue;
                default:
                    return Color.White;
            }
        }
    }
}
