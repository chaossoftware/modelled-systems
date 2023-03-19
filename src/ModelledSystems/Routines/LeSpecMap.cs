using System;
using System.Linq;
using System.Threading.Tasks;
using ChaosSoft.Core.DataUtils;
using ChaosSoft.NumericalMethods.Equations;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.Orthogonalization;
using ScottPlot.Drawing;

namespace ModelledSystems.Routines;

internal class LeSpecMap : Routine
{
    private readonly double xBegin, yBegin, xStep, yStep, xEnd, yEnd;
    private readonly int _xParamIndex, _yParamIndex;
    private readonly double[,] _arr;
    private readonly double[,] _arrPvc;

    private readonly TaskProgress _progress;
    
    private readonly int _iterations;

    private readonly SysParamCfg _xParam, _yParam;

    private readonly string _ortType;
    private readonly int _irate;

    public LeSpecMap(string outDir, SystemCfg sysConfig, int xParamIndex, int yParamIndex, int paramIterations, OrthogonalizationCfg orthogonalization) 
        : base(outDir, sysConfig)
    {
        _xParamIndex = xParamIndex;
        _yParamIndex = yParamIndex;
        _iterations = paramIterations;

        _xParam = SysConfig.Params[xParamIndex];
        _yParam = SysConfig.Params[yParamIndex];

        xBegin = _xParam.From;
        yBegin = _yParam.From;
        xEnd = _xParam.To;
        yEnd = _yParam.To;
        xStep = (xEnd - xBegin) / _iterations;
        yStep = (yEnd - yBegin) / _iterations;

        _progress = new TaskProgress(_iterations * _iterations);

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
        var plt = GetPlot(_xParam.Name, _yParam.Name);

        int maxPositiveLeIndex = (int)Matrix.Max(_arr);
        int minLeIndex = (int)Matrix.Min(_arr);
        
        var cm = new Colormap(new LeSpecColormap(minLeIndex, maxPositiveLeIndex));
        var hm = plt.AddHeatmap(_arr, cm, lockScales: false);
        var cb = plt.AddColorbar(hm, 50);
        plt.Margins(0, 0);

        double[] ticks = Vector.CreateUniform(maxPositiveLeIndex + 1 - minLeIndex, minLeIndex, 1d);

        cb.SetTicks(
            ticks,
            ticks.Select(t => t == -1 ? "X" : t.ToString()).ToArray(), 
            min: minLeIndex, 
            max: maxPositiveLeIndex + 1);

        plt.XTicks(new double[] { 0, _iterations }, new string[] { xBegin.ToString(), xEnd.ToString() });
        plt.YTicks(new double[] { 0, _iterations }, new string[] { yBegin.ToString(), yEnd.ToString() });

        string fName = $"_lyapunov_map_{_xParam.Name}_{_yParam.Name}.png";

        plt.SaveFig(FileNameBase + fName);
    }

    public void Func(int z)
    {
        long totIter;
        double[] _rMatrix;

        int x = z / _iterations;
        int y = z % _iterations;

        double[] vars = SysConfig.ParamsValues.ToArray();
        
        vars[_xParamIndex] = xBegin + xStep * x;
        vars[_yParamIndex] = yBegin + yStep * y;

        SystemBase equations = GetLinearizedSystemEquations(vars);

        var solver = GetSolver(equations);

        totIter = (long)(SysConfig.Solver.ModellingTime / solver.Dt);
        OrthogonalizationBase ort = GetOrthogonalization(_ortType, equations.Count);
        LeSpecBenettin lyap = new LeSpecBenettin(equations.Count);

        _rMatrix = new double[equations.Count];

        for (int i = 0; i < totIter; i++)
        {
            solver.NexStep();

            for (int j = 0; j < _irate; j++)
            {
                solver.NexStep();
            }

            ort.Perform(solver.Solution, _rMatrix);
            lyap.CalculateLyapunovSpectrum(_rMatrix, solver.Time);

            if (_rMatrix.Any(v => double.IsNaN(v) || double.IsInfinity(v)))
            {
                Vector.FillWith(lyap.Result, double.NaN);
                break;
            }

            totIter--;
        }

        int rez = lyap.Result.Any(e => double.IsNaN(e)) 
            ? -1 
            : lyap.Result.Count(l => l > 1e-4);

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

public class LeSpecColormap : IColormap
{
    private readonly int[] fullPalette =
    {
        16777215, // white
        03394611, //green
        00039423, //blue
        16750899, //orange
        16711935, //fuchsia
    };

    private readonly int[] rgb;

    public LeSpecColormap(int minLeIndex, int positiveLeCount)
    {
        int[] tmp = fullPalette.Skip(minLeIndex + 1).Take(positiveLeCount + 1 - minLeIndex).ToArray();

        rgb = new int[256];
        int coeff = rgb.Length / tmp.Length;

        for (int i = 0; i < rgb.Length; i++)
        {
            int ind = Math.Min(i / coeff, tmp.Length - 1);
            rgb[i] = tmp[ind];
        }
    }

    public string Name => "LeSpec";

    public (byte r, byte g, byte b) GetRGB(byte value)
    {
        byte[] bytes = BitConverter.GetBytes(rgb[value]);
        return (bytes[2], bytes[1], bytes[0]);
    }
}
