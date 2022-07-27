using ChaosSoft.Core;
using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines
{
    class BenettinLLEParam : Routine
    {

        static Timeseries SyncMapSeries;
        int step, currentIteration;
        int ParamIndex;
        static int TotalIterations;
        ConcurrentBag<DataPoint> ds;
        Parameter Param;

        public BenettinLLEParam(string outDir, SystemParameters systemParameters, int paramIndex) : base(outDir, systemParameters)
        {
            SyncMapSeries = new Timeseries();
            currentIteration = 1;
            ParamIndex = paramIndex;
            Param = SysParameters.ListParameters[ParamIndex];
            TotalIterations = (int)((Param.End - Param.Start) / Param.Step);
            step = TotalIterations / Console.BufferWidth;
            ds = new ConcurrentBag<DataPoint>();
        }

        public override void Run()
        {
            Parallel.For(0, TotalIterations, i =>
            {
                Func(Param.Start + Param.Step * i);
            });

            SyncMapSeries.DataPoints.AddRange(ds);
            SyncMapSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

            DataWriter.CreateDataFile(Path.Combine(OutDir, SysParameters.SystemName + "_data_lyapunov_param_" + Param.Name), SyncMapSeries.ToString());


            var plt = new ScottPlot.Plot(Size.Width, Size.Height);
            plt.AddSignalXY(SyncMapSeries.XValues, SyncMapSeries.YValues);
            plt.XAxis.Label(Param.Name);
            plt.YAxis.Label("LLE");
            plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_param_" + Param.Name + ".png"));
        }


        private void Func(double p)
        {
            double[] vars;

            vars = SysParameters.Defaults;
            vars[ParamIndex] = p;

            double EqStep = SysParameters.Step.Default;

            var eq = GetSystemEquations(false, vars, EqStep);
            var eq1 = GetSystemEquations(false, vars, EqStep);

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

                    for (int _i = 1; _i < eq.EquationsCount; _i++)
                    {
                        eq1.Solver.Solution[0, _i] = eq.Solver.Solution[0, _i];
                    }

                    lsum = 0;
                    nl = 0;
                    continue;
                }

                eq1.Solver.NexStep();

                double dl2 = 0;

                for (int _i = 0; _i < eq.EquationsCount; _i++)
                {
                    dl2 += FastMath.Pow2(eq1.Solver.Solution[0, _i] - eq.Solver.Solution[0, _i]);
                }

                if (dl2 > 0)
                {
                    double df = 1e16 * dl2;
                    double rs = 1 / Math.Sqrt(df);

                    for (int _i = 0; _i < eq.EquationsCount; _i++)
                    {
                        eq1.Solver.Solution[0, _i] = eq.Solver.Solution[0, _i] + rs * (eq1.Solver.Solution[0, _i] - eq.Solver.Solution[0, _i]);
                    }

                    lsum += Math.Log(df);
                    nl++;
                }

                l1 = 0.5 * lsum / nl / Math.Abs(eq.Solver.Step);
            }

            ds.Add(new DataPoint(p, l1));

            if (currentIteration++ % step == 0)
            {
                Console.Write("#");
            }
        }

    }
}
