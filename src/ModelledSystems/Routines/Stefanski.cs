using ChaosSoft.Core.Data;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ModelledSystems.Routines
{
    class Stefanski : Routine
    {

        static Timeseries SyncMapSeries;
        int step, currentIteration;
        int Piter;
        double Pstep;
        static int TotalIterations;
        int LastIter = 100;
        ConcurrentBag<DataPoint> ds;

        public Stefanski(string outDir, SystemParameters systemParameters, int pIter, double pstep) : base (outDir, systemParameters)
        {
            SyncMapSeries = new Timeseries();
            currentIteration = 1;
            Piter = pIter;
            Pstep = pstep;
            step = Piter / Console.BufferWidth;
            TotalIterations = (int)(SysParameters.ModellingTime * SysParameters.Step.Default);
            ds = new ConcurrentBag<DataPoint>();
        }

        public override void Run()
        {
            Parallel.For(0, Piter, i =>
            {
                Func(Pstep * i);
            });

            SyncMapSeries.DataPoints.AddRange(ds);

            SyncMapSeries.DataPoints.Sort(delegate (DataPoint c1, DataPoint c2) { try { return c1.X.CompareTo(c2.X); } catch { } return 0; });

            //DataWriter.CreateDataFile("fileName", SyncMapSeries.ToString());


            var plt = new ScottPlot.Plot(Size.Width, Size.Height);
            plt.XAxis.Label("p");
            plt.YAxis.Label("Δ");

            foreach (DataPoint dp in SyncMapSeries.DataPoints)
            {
                plt.AddPoint(dp.X, dp.Y, Color.Blue, 1);
            }

            plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_stefanski.png"));

            int k = SyncMapSeries.Length - 1;
            double rezY = SyncMapSeries.DataPoints[k].Y;
            bool sync = true;

            while (sync)
            {
                sync = Math.Abs(SyncMapSeries.DataPoints[--k].Y - rezY) < 1e-8;
            }

            Console.WriteLine(SyncMapSeries.DataPoints[k++].X.ToString("F5"));
        }


        private void Func(double p)
        {
            AugmentedEquations AugmentedEquations = GetSystemEquations();
            AugmentedEquations.p = p;
            AugmentedEquations.Solver.Step = SysParameters.Step.Default;
            AugmentedEquations.Solver.Init();

            for (int j = 0; j < TotalIterations; j++)
            {
                AugmentedEquations.Solver.NexStep();
                if (j > TotalIterations - LastIter)
                {
                    double rez = AugmentedEquations.Solver.Solution[0, AugmentedEquations.EquationsCount - AugmentedEquations.EquationsCount / 3];
                    if (!double.IsInfinity(rez) && !double.IsNaN(rez) && rez < 100 && rez > -100)
                        ds.Add(new DataPoint(p, rez));
                }
            }

            if (currentIteration++ % step == 0)
            {
                Console.Write("#");
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
