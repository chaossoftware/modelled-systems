using MathLib.Data;
using MathLib.DrawEngine.Charts;
using MathLib.IO;
using MathLib.MathMethods.Solvers;
using MathLib.Threading;
using System;
using System.Collections.Concurrent;
using System.Drawing.Imaging;
using System.IO;

namespace ModelledSystems.Routines
{
    class Bifurcation : Routine
    {

        static Timeseries SyncMapSeries;
        int step, currentIteration;
        int ParamIndex;
        static int TotIter;
        int LastIter = 100;
        ConcurrentBag<DataPoint> ds;
        Parameter Param;

        public Bifurcation(string outDir, SystemParameters systemParameters, int paramIndex) : base(outDir, systemParameters)
        {
            SyncMapSeries = new Timeseries();
            currentIteration = 1;
            ParamIndex = paramIndex;
            Param = SysParameters.ListParameters[ParamIndex];
            TotIter = (int)((Param.End - Param.Start) / Param.Step);
            step = TotIter / Console.BufferWidth;
            ds = new ConcurrentBag<DataPoint>();
        }

        public override void Run()
        {
            ThreadedRun threadedRun = new ThreadedRun();


            for (double p = Param.Start; p < Param.End; p += Param.Step)
            {
                threadedRun.RunOnSeparateProcessor(Func, new object[1] { p });

                if (currentIteration++ % step == 0)
                    Console.Write("#");
            }

            threadedRun.WaitForAllTasks();

            SyncMapSeries.DataPoints.AddRange(ds);

            DataWriter.CreateDataFile(Path.Combine(OutDir, SysParameters.SystemName + "_dataBifur_" + Param.Name), SyncMapSeries.ToString());

            var po = new ScatterPlot(Size, SyncMapSeries);
            po.LabelX = Param.Name;
            po.LabelY = "X";
            po.Plot().Save(Path.Combine(OutDir, SysParameters.SystemName + "_bifur_" + Param.Name + ".png"), ImageFormat.Png);
        }


        private void Func(object[] parameters)
        {
            double p = (double)parameters[0];

            double[] vars;

            vars = SysParameters.Defaults;
            vars[ParamIndex] = p;

            SystemEquations eq = GetSystemEquations(false, vars, SysParameters.Step.Default);
            eq.Solver.Init();

            for (int j = 0; j < TotIter; j++)
            {
                eq.Solver.NexStep();
                if (j > TotIter - LastIter)
                {
                    double rez = eq.Solver.Solution[0, 0];
                    if (!double.IsInfinity(rez) && !double.IsNaN(rez) && Math.Abs(rez) < 1000)
                        ds.Add(new DataPoint(p, rez));
                }
            }
        }
    }
}
