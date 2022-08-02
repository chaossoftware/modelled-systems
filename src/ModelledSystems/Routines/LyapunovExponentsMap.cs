using System;
using System.Drawing;
using System.IO;
using System.Linq;
using ChaosSoft.Core;
using ChaosSoft.Core.NumericalMethods.Lyapunov;
using ChaosSoft.Core.NumericalMethods.Orthogonalization;
using ChaosSoft.Core.NumericalMethods.Solvers;
using ChaosSoft.Core.Threading;

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

            arr = new double[yIterations, xIterations];

            for (int i = 0; i < yIterations; i++)
            {
                for (int j = 0; j < xIterations; j++)
                {
                    arr[i, j] = -1;
                }
            }
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
                    threadedRun.RunOnSeparateProcessor(() => Func(xVal, yVal, x, y));

                    if (currentIteration++ % step == 0)
                        Console.Write("#");
                }
            }

            threadedRun.WaitForAllTasks();

            GetImage();
        }

        private void GetImage()
        {
            var plt = new ScottPlot.Plot(Size.Width, Size.Height);
            
            plt.XAxis.Label(xParameter.Name);
            plt.YAxis.Label(yParameter.Name);

            var hm = plt.AddHeatmap(arr, lockScales: false);
            var cb = plt.AddColorbar(hm);
            hm.Smooth = true;
            plt.Margins(0, 0);

            double maxPositiveLeIndex = Ext.Max(arr);

            double[] ticks = ArrayUtil.GenerateArray((int)maxPositiveLeIndex + 1, 0d, 1d);

            cb.SetTicks(
                ticks,
                ticks.Select(t => t.ToString()).ToArray(), 
                min: 0, 
                max: maxPositiveLeIndex);

            plt.XTicks(new double[] { 0, xIterations }, new string[] { xBegin.ToString(), xEnd.ToString() });
            plt.YTicks(new double[] { 0, yIterations }, new string[] { yBegin.ToString(), yEnd.ToString() });

            plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_map.png"));
        }

        public void Func(double xparam, double yParam, int x, int y)
        {
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
                lyap.CalculateLyapunovSpectrum(R, equations.Solver.Time);
                totIter--;
            }

            int rez = lyap.Result.Spectrum.Count(l => l > 0);

            arr[yIterations - 1 - y, x] = rez;
        }
    }
}
