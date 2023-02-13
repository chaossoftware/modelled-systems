using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChaosSoft.Core.Extensions;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.Orthogonalization;

namespace ModelledSystems.Routines;

internal class LesMap : Routine
{
    private readonly double xBegin, yBegin, xStep, yStep, xEnd, yEnd;
    private readonly int _xParamIndex, _yParamIndex;
    private readonly double[,] _arr;
    private readonly double[,] _arrPvc;

    private readonly TaskProgress _progress;
    
    private readonly int _iterations;
    private readonly double _eqStep;

    private readonly Parameter xParameter, yParameter;

    public LesMap(string outDir, SystemParameters systemParameters, int xParamIndex, int yParamIndex, int paramIterations) 
        : base(outDir, systemParameters)
    {
        _xParamIndex = xParamIndex;
        _yParamIndex = yParamIndex;
        _iterations = paramIterations;
        _eqStep = SysParameters.Step;

        xParameter = SysParameters.ListParameters[xParamIndex];
        yParameter = SysParameters.ListParameters[yParamIndex];

        xBegin = xParameter.Start;
        yBegin = yParameter.Start;
        xStep = (xEnd - xBegin) / _iterations;
        yStep = (yEnd - yBegin) / _iterations;
        xEnd = xParameter.End;
        yEnd = yParameter.End;

        var totalIterations = _iterations * _iterations;

        _progress = new TaskProgress(totalIterations);

        _arr = new double[_iterations, _iterations];
        Matrix.FillWith(_arr, -1);

        _arrPvc = new double[_iterations, _iterations];
    }

    public override void Run()
    {
        Parallel.For(0, _iterations * _iterations, z => Func(z));
        GetImage();
    }

    private void GetImage()
    {
        var plt = new ScottPlot.Plot(Size.Width, Size.Height);
        
        plt.XAxis.Label(xParameter.Name);
        plt.YAxis.Label(yParameter.Name);

        int maxPositiveLeIndex = (int)Matrix.Max(_arr);
        int minLeIndex = (int)Matrix.Min(_arr);
        MakeGradient(maxPositiveLeIndex);

        var hm = plt.AddHeatmap(_arr, ScottPlot.Drawing.Colormap.Jet, lockScales: false);
        var cb = plt.AddColorbar(hm);
        hm.Smooth = true;
        plt.Margins(0, 0);


        double[] ticks = Vector.CreateUniform(maxPositiveLeIndex + 1, minLeIndex, 1d);

        cb.SetTicks(
            ticks,
            ticks.Select(t => t.ToString()).ToArray(), 
            min: minLeIndex, 
            max: maxPositiveLeIndex);

        plt.XTicks(new double[] { 0, _iterations }, new string[] { xBegin.ToString(), xEnd.ToString() });
        plt.YTicks(new double[] { 0, _iterations }, new string[] { yBegin.ToString(), yEnd.ToString() });

        plt.SaveFig(Path.Combine(OutDir, SysParameters.SystemName + "_lyapunov_map.png"));
    }

    public void Func(int z /*double xparam, double yParam, int x, int y*/)
    {
        long totIter;
        double[] R;

        int x = z / _iterations;
        int y = z % _iterations;

        double[] vars = new double[SysParameters.Defaults.Length];
        Array.Copy(SysParameters.Defaults, vars, vars.Length);
        vars = SysParameters.Defaults;
        vars[_xParamIndex] = xBegin + xStep * x;
        vars[_yParamIndex] = yBegin + yStep * y;

        SystemBase equations = GetLinearizedSystemEquations(vars);

        var solver = GetSolver(SysParameters.Solver, equations, _eqStep);

        totIter = (long)(SysParameters.ModellingTime / solver.Dt);
        OrthogonalizationBase ort = new ModifiedGrammSchmidt(equations.Count);
        BenettinMethod lyap = new BenettinMethod(equations.Count);

        R = new double[equations.Count];

        for (int i = 0; i < totIter; i++)
        {
            solver.NexStep();
            ort.Perform(solver.Solution, R);
            lyap.CalculateLyapunovSpectrum(R, solver.Time);
            totIter--;
        }

        int rez = lyap.Result.Count(l => l > 0);

            _arr[_iterations - 1 - y, x] = rez;
            _arrPvc[_iterations - 1 - y, x] = StochasticProperties.PhaseVolumeContractionSpeed(lyap.Result);

        _progress.Iterate();
    }

    private void MakeGradient(int maxLeCount)
    {
        double maxCoeff = 0.5d;
        int arrLength = maxLeCount + 1;
        double[] mins = new double[arrLength];
        double[] maxs = new double[arrLength];
        double[] coeffs = new double[arrLength];

        Vector.FillWith(mins, double.MaxValue);
        Vector.FillWith(maxs, double.MinValue);

        for (int x = 0; x < _iterations; x++)
        {
            for (int y = 0; y < _iterations; y++)
            {
                for (int i = 0; i < arrLength; i++)
                {
                    if (double.IsInfinity(_arrPvc[y, x]))
                    {
                        continue;
                    }

                    if (_arrPvc[y, x] < mins[i])
                    {
                        mins[i] = _arrPvc[y, x];
                    }

                    if (_arrPvc[y, x] > maxs[i])
                    {
                        maxs[i] = _arrPvc[y, x];
                    }
                }
            }
        }

        for (int i = 0; i < arrLength; i++)
        {
            coeffs[i] = maxCoeff / (maxs[i] - mins[i]);
        }

        for (int x = 0; x < _iterations; x++)
        {
            for (int y = 0; y < _iterations; y++)
            {
                if (double.IsInfinity(_arrPvc[y, x]))
                {
                    continue;
                }

                if (_arr[y, x] == -1)
                {
                    continue;
                }
                
                int lec = (int)_arr[y, x];
                _arr[y, x] += (_arrPvc[y, x] - mins[lec]) * coeffs[lec] - maxCoeff / 2;
            }
        }

    }
}
