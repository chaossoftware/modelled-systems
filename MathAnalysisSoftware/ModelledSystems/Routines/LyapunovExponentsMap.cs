using MathLib.DrawEngine.Charts;
using MathLib.MathMethods.Lyapunov;
using MathLib.MathMethods.Orthogonalization;
using MathLib.MathMethods.Solvers;
using MathLib.Threading;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ModelledSystems.Routines
{
    class LyapunovExponentsMap : Routine
    {

        double Xbegin, Ybegin, Xstep, Ystep, Xend, Yend;
        int XparamIndex, YparamIndex;
        private static double[,] arr;

        int TotIter, Xiterations, Yiterations;
        double EqStep;

        static int step, currentIteration;

        Parameter Xparam, Yparam;

        public LyapunovExponentsMap(string outDir, SystemParameters systemParameters, int xParamIndex, int yParamIndex) : base(outDir, systemParameters)
        {
            XparamIndex = xParamIndex;
            YparamIndex = yParamIndex;
            EqStep = SysParameters.Step.Default;

            Xparam = SysParameters.ListParameters[xParamIndex];
            Yparam = SysParameters.ListParameters[yParamIndex];

            Xbegin = Xparam.Start;
            Ybegin = Yparam.Start;
            Xstep = Xparam.Step;
            Ystep = Yparam.Step;
            Xend = Xparam.End;
            Yend = Yparam.End;

            Xiterations = (int)((Xend - Xbegin) / Xstep);
            Yiterations = (int)((Yend - Ybegin) / Ystep);

            currentIteration = 1;
            TotIter = Xiterations * Yiterations;

            step = TotIter / Console.BufferWidth;

            arr = new double[Xiterations, Yiterations];

            for (int i = 0; i < Xiterations; i++)
                for (int j = 0; j < Yiterations; j++)
                    arr[i, j] = -1;
        }


        public override void Run()
        {
            ThreadedRun threadedRun = new ThreadedRun();
            double yVal;
            double xVal = Xbegin;

            for (int x = 0; x < Xiterations; x++)
            {
                xVal += Xstep;
                yVal = Ybegin;
                for (int y = 0; y < Yiterations; y++)
                {
                    yVal += Ystep;
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
            po.Xmin = Xbegin;
            po.Xmax = Xend;
            po.Ymin = Ybegin;
            po.Ymax = Yend;
            po.LabelX = Xparam.Name;
            po.LabelY = Yparam.Name;
            po.Plot().Save(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_map.png"), ImageFormat.Png);
        }


        public void Func(object[] parameters)
        {

            double xparam = (double)parameters[0], yParam = (double)parameters[1];
            int _x = (int)parameters[2], _y = (int)parameters[3];

            int rez = 0;
            long _totIter;
            double[] R, vars;

            vars = SysParameters.Defaults;
            vars[XparamIndex] = xparam;
            vars[YparamIndex] = yParam;

            SystemEquations equations = GetSystemEquations(true, vars, EqStep);
            _totIter = (long)(SysParameters.ModellingTime / equations.Solver.Step);
            Orthogonalization ort = new MGS(equations.N);
            BenettinMethod lyap = new BenettinMethod(equations.N);

            R = new double[equations.N];
            equations.Solver.Init();

            for (int i = 0; i < _totIter; i++)
            {
                equations.Solver.NexStep();
                ort.makeOrthogonalization(equations.Solver.Solution, R);
                lyap.calculateLE(R, equations.Solver.Time);
                _totIter--;
            }

            for (int k = 0; k < equations.N; k++)
                if (lyap.lespec[k] > 0)
                    rez++;

            arr[_x, _y] = rez;
        }
    }

    class ColorCondition4 : MathLib.DrawEngine.Charts.ColorMaps.ColorMap
    {
        public Color GetColor(double i)
        {
            if (i == 3) return Color.Black;
            if (i == 2) return Color.DarkGreen;
            if (i == 1) return Color.Green;
            if (i == 0) return Color.Blue;
            return Color.White;
        }
    }
}
