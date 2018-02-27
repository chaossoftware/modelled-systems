using MathLib.Data;
using MathLib.DrawEngine;
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
    class BenettinLLEParam : Routine
    {

        static DataSeries SyncMapSeries;
        int step, currentIteration;
        int ParamIndex;
        static int TotIter;
        ConcurrentBag<DataPoint> ds;
        Parameter Param;

        public BenettinLLEParam(string outDir, SystemParameters systemParameters, int paramIndex) : base(outDir, systemParameters)
        {
            SyncMapSeries = new DataSeries();
            currentIteration = 1;
            ParamIndex = paramIndex;
            Param = SysParameters.ListParameters[ParamIndex];
            TotIter = (int)((Param.End - Param.Start)/Param.Step);
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

            SyncMapSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

            DataWriter.CreateDataFile(Path.Combine(OutDir, SysParameters.SystemName + "_data_lyapunov_param_" + Param.Name), SyncMapSeries.ToString());

            SignalPlot po = new SignalPlot(SyncMapSeries, Size, 1);
            po.LabelX = Param.Name;
            po.LabelY = "LLE";
            po.Plot().Save(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_param_" + Param.Name + ".png"), ImageFormat.Png);
        }


        private void Func(object[] parameters)
        {
            double p = (double)parameters[0];

            double[] vars;

            vars = SysParameters.Defaults;
            vars[ParamIndex] = p;

            double EqStep = SysParameters.Step.Default;

            SystemEquations eq = GetSystemEquations(false, vars, EqStep);
            SystemEquations eq1 = GetSystemEquations(false, vars, EqStep);

            int EqN = eq.N;

            long _totIter = (long)(SysParameters.ModellingTime / EqStep);
            eq.Solver.Init();

            double l1 = 0, lsum = 0;
            long nl = 0;

            for (int i = 0; i < _totIter; i++)
            {
                eq.Solver.NexStep();

                if (eq1.Solver.Solution[0, 0] == 0)
                {
                    eq1.Solver.Solution[0, 0] += eq.Solver.Solution[0, 0] + 1e-8;
                    for (int _i = 1; _i < eq.N; _i++)
                        eq1.Solver.Solution[0, _i] = eq.Solver.Solution[0, _i];
                    lsum = 0;
                    nl = 0;
                    continue;
                }

                eq1.Solver.NexStep();

                double dl2 = 0;
                for (int _i = 0; _i < eq.N; _i++)
                    dl2 += Math.Pow(eq1.Solver.Solution[0, _i] - eq.Solver.Solution[0, _i], 2);

                if (dl2 > 0)
                {
                    double df = 1e16 * dl2;
                    double rs = 1 / Math.Sqrt(df);

                    for (int _i = 0; _i < eq.N; _i++)
                        eq1.Solver.Solution[0, _i] = eq.Solver.Solution[0, _i] + rs * (eq1.Solver.Solution[0, _i] - eq.Solver.Solution[0, _i]);
                    lsum += Math.Log(df);
                    nl++;
                }

                l1 = 0.5 * lsum / nl / Math.Abs(eq.Solver.Step);
            }

            ds.Add(new DataPoint(p, l1));
        }

    }
}
