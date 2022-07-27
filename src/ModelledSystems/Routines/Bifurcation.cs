﻿using ChaosSoft.Core.Data;
using ChaosSoft.Core.IO;
using ChaosSoft.Core.NumericalMethods.Solvers;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines
{
    class Bifurcation : Routine
    {

        static Timeseries SyncMapSeries;
        int step, currentIteration;
        int ParamIndex;
        static int TotalIterations;
        int LastIter = 100;
        ConcurrentBag<DataPoint> ds;
        Parameter Param;

        public Bifurcation(string outDir, SystemParameters systemParameters, int paramIndex) : base(outDir, systemParameters)
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

            DataWriter.CreateDataFile(Path.Combine(OutDir, SysParameters.SystemName + "_dataBifur_" + Param.Name), SyncMapSeries.ToString());

            var plt = new ScottPlot.Plot(Size.Width, Size.Height);
            plt.XAxis.Label(Param.Name);
            plt.YAxis.Label("X");

            foreach (DataPoint dp in SyncMapSeries.DataPoints)
            {
                plt.AddPoint(dp.X, dp.Y, Color.Blue, 1);
            }

            plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_bifur_" + Param.Name + ".png"));
        }


        private void Func(double p)
        {
            double[] vars;

            vars = SysParameters.Defaults;
            vars[ParamIndex] = p;

            SystemEquations eq = GetSystemEquations(false, vars, SysParameters.Step.Default);
            eq.Solver.Init();

            for (int j = 0; j < TotalIterations; j++)
            {
                eq.Solver.NexStep();
                if (j > TotalIterations - LastIter)
                {
                    double rez = eq.Solver.Solution[0, 0];
                    if (!double.IsInfinity(rez) && !double.IsNaN(rez) && Math.Abs(rez) < 1000)
                        ds.Add(new DataPoint(p, rez));
                }
            }

            if (currentIteration++ % step == 0)
            {
                Console.Write("#");
            }
        }
    }
}
