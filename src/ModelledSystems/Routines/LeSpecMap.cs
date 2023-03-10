using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChaosSoft.Core.DataUtils;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.Orthogonalization;

namespace ModelledSystems.Routines;

internal class LeSpecMap : Routine
{
    private readonly double xBegin, yBegin, xStep, yStep, xEnd, yEnd;
    private readonly int _xParamIndex, _yParamIndex;
    private readonly double[,] _arr;
    private readonly double[,] _arrPvc;

    private readonly TaskProgress _progress;
    
    private readonly int _iterations;
    private readonly double _eqStep;

    private readonly SysParamCfg xParameter, yParameter;

    private readonly string _ortType;
    private readonly int _irate;

    public LeSpecMap(string outDir, SystemCfg sysConfig, int xParamIndex, int yParamIndex, int paramIterations, OrthogonalizationCfg orthogonalization) 
        : base(outDir, sysConfig)
    {
        _xParamIndex = xParamIndex;
        _yParamIndex = yParamIndex;
        _iterations = paramIterations;
        _eqStep = SysConfig.Solver.Dt;

        xParameter = SysConfig.Params[xParamIndex];
        yParameter = SysConfig.Params[yParamIndex];

        xBegin = xParameter.From;
        yBegin = yParameter.From;
        xEnd = xParameter.To;
        yEnd = yParameter.To;
        xStep = (xEnd - xBegin) / _iterations;
        yStep = (yEnd - yBegin) / _iterations;

        var totalIterations = _iterations * _iterations;

        _progress = new TaskProgress(totalIterations);

        _arr = new double[_iterations, _iterations];
        Matrix.FillWith(_arr, -1);

        _ortType = orthogonalization.Type;
        _irate = orthogonalization.Interval;

        _arrPvc = new double[_iterations, _iterations];
    }

    public override void Run()
    {
        Parallel.For(0, _iterations * _iterations, z => Func(z));
        GetImage();
    }

    private void GetImage()
    {
        var plt = GetPlot(xParameter.Name, yParameter.Name);

        int maxPositiveLeIndex = (int)Matrix.Max(_arr);
        int minLeIndex = (int)Matrix.Min(_arr);
        MakeGradient(maxPositiveLeIndex);

        var hm = plt.AddHeatmap(_arr, ScottPlot.Drawing.Colormap.Turbo, lockScales: false);
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

        string fName = $"_lyapunov_map_{xParameter.Name}_{yParameter.Name}.png";

        plt.SaveFig(Path.Combine(OutDir, SysConfig.Name + fName));
    }

    public void Func(int z)
    {
        long totIter;
        double[] R;

        int x = z / _iterations;
        int y = z % _iterations;

        double[] vars = new double[SysConfig.ParamsValues.Length];
        Array.Copy(SysConfig.ParamsValues, vars, vars.Length);
        
        vars[_xParamIndex] = xBegin + xStep * x;
        vars[_yParamIndex] = yBegin + yStep * y;

        SystemBase equations = GetLinearizedSystemEquations(vars);

        var solver = GetSolver(equations);

        totIter = (long)(SysConfig.Solver.ModellingTime / solver.Dt);
        OrthogonalizationBase ort = GetOrthogonalization(_ortType, equations.Count);
        LeSpecBenettin lyap = new LeSpecBenettin(equations.Count);

        R = new double[equations.Count];

        for (int i = 0; i < totIter; i++)
        {
            solver.NexStep();

            for (int j = 0; j < _irate; j++)
            {
                solver.NexStep();
            }

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
