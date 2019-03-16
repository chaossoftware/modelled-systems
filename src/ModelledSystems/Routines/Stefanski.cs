using MathLib.Data;
using MathLib.DrawEngine;
using MathLib.DrawEngine.Charts;
using MathLib.Threading;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ModelledSystems.Routines
{
    class Stefanski : Routine
    {

        static Timeseries SyncMapSeries;
        int step, currentIteration;
        int Piter;
        double Pstep;
        static int TotIter;
        int LastIter = 100;
        ConcurrentBag<DataPoint> ds;

        public Stefanski(string outDir, SystemParameters systemParameters, int pIter, double pstep) : base (outDir, systemParameters)
        {
            SyncMapSeries = new Timeseries();
            currentIteration = 1;
            Piter = pIter;
            Pstep = pstep;
            step = Piter / Console.BufferWidth;
            TotIter = (int)(SysParameters.ModellingTime * SysParameters.Step.Default);
            ds = new ConcurrentBag<DataPoint>();
        }

        public override void Run()
        {
            ThreadedRun threadedRun = new ThreadedRun();


            for (double p = 0; p < Piter * Pstep; p += Pstep)
            {
                threadedRun.RunOnSeparateProcessor(Func, new object[1] { p });

                if (currentIteration++ % step == 0)
                    Console.Write("#");
            }

            SyncMapSeries.DataPoints.AddRange(ds);

            SyncMapSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

            //DataWriter.CreateDataFile("fileName", SyncMapSeries.ToString());

            Size size = new Size(320, 240);
            var po = new ScatterPlot(size, SyncMapSeries);
            po.LabelX = "p";
            po.LabelY = "Δ";
            po.Plot().Save(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_stefanski.png"), ImageFormat.Png);

            int k = SyncMapSeries.Length - 1;
            double rezY = SyncMapSeries.DataPoints[k].Y;
            bool sync = true;

            while (sync)
            {
                sync = Math.Abs(SyncMapSeries.DataPoints[--k].Y - rezY) < 1e-8;
            }

            Console.WriteLine(SyncMapSeries.DataPoints[k++].X.ToString("F5"));
        }


        private void Func(object[] parameters)
        {
            double p = (double)parameters[0];

            AugmentedEquations AugmentedEquations = GetSystemEquations();
            AugmentedEquations.p = p;
            AugmentedEquations.Solver.Step = SysParameters.Step.Default;
            AugmentedEquations.Solver.Init();

            for (int j = 0; j < TotIter; j++)
            {
                AugmentedEquations.Solver.NexStep();
                if (j > TotIter - LastIter)
                {
                    double rez = AugmentedEquations.Solver.Solution[0, AugmentedEquations.EquationsCount - AugmentedEquations.EquationsCount / 3];
                    if (!double.IsInfinity(rez) && !double.IsNaN(rez) && rez < 100 && rez > -100)
                        ds.Add(new DataPoint(p, rez));
                }
            }
        }


        private AugmentedEquations GetSystemEquations()
        {
            switch (SysParameters.SystemName.ToLower())
            {
                case "lorenz":
                    return new LorenzAugmented();
                case "rossler":
                    return new RosslerAugmented();
                case "henon":
                    return new HenonAugmented();
                case "henon_generalized":
                    return new HenonGeneralizedAugmented();
                case "logistic":
                    return new LogisticAugmented();
                default:
                    throw new ArgumentException();
            }
        }

    }
}
