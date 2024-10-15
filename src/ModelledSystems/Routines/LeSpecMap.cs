using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ChaosSoft.Core.DataUtils;
using ChaosSoft.NumericalMethods.Ode.Linearized;
using ChaosSoft.NumericalMethods.Lyapunov;
using ChaosSoft.NumericalMethods.QrDecomposition;
using ScottPlot.Drawing;

namespace ModelledSystems.Routines;

internal sealed class LeSpecMap : Routine
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
        MakeGradient(maxPositiveLeIndex + 1 - (minLeIndex == -1 ? 0 : minLeIndex));

        var cm = new Colormap(new LeSpecColormap(minLeIndex, maxPositiveLeIndex));
        var hm = plt.AddHeatmap(_arr, cm, lockScales: false);
        var cb = plt.AddColorbar(hm, 50);
        plt.Margins(0, 0);

        double[] ticks = Vector.CreateUniform((maxPositiveLeIndex + 1 - minLeIndex) * 2 + 1, minLeIndex, 0.5);
        string[] tickMarks = new string[ticks.Length];

        for (int i = 0; i < tickMarks.Length; i++)
        {
            if (i % 2 == 1)
            {
                tickMarks[i] = ticks[i - 1] == -1 ? "X" : ticks[i - 1].ToString();
            }
            else
            {
                tickMarks[i] = string.Empty;
            }
        }

        cb.SetTicks(
            ticks,
            tickMarks,//ticks.Select(t => t == -1 ? "X" : t.ToString()).ToArray(), 
            min: minLeIndex, 
            max: maxPositiveLeIndex + 1);

        plt.XTicks(new double[] { 0, _iterations }, new string[] { xBegin.ToString(), xEnd.ToString() });
        plt.YTicks(new double[] { 0, _iterations }, new string[] { yBegin.ToString(), yEnd.ToString() });

        string fName = $"_lyapunov_map_{_xParam.Name}_{_yParam.Name}.png";

        SavePlot(plt, FileNameBase + fName);
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

        ILinearizedOdeSys equations = GetLinearizedSystemEquations(vars);

        LinearizedOdeSolverBase solver = GetLinearizedSolver(equations);
        solver.SetInitialConditions(0, GetInitialConditions());
        solver.SetLinearInitialConditions(SysConfig.LinearInitialConditions);

        totIter = (long)(SysConfig.Solver.ModellingTime / solver.Dt);
        IQrDecomposition ort = GetOrthogonalization(_ortType, equations.EqCount);
        LeSpecBenettin lyap = new LeSpecBenettin(equations.EqCount);

        _rMatrix = new double[equations.EqCount];

        for (int i = 0; i < totIter; i++)
        {
            for (int j = 0; j < _irate; j++)
            {
                solver.NextStep();
            }

            if (solver.IsSolutionDecayed())
            {
                Vector.FillWith(lyap.Result, double.NaN);
                break;
            }

            _rMatrix = ort.Perform(solver.Linearization);
            lyap.CalculateLyapunovSpectrum(_rMatrix, solver.T);
        }

        int rez = lyap.Result.Any(e => double.IsNaN(e)) 
            ? -1 
            : lyap.Result.Count(l => l > 9e-3);

        //if (rez > 1)
        //{
        //    Console.WriteLine(Format.General(lyap.Result));
        //}

        _arr[_iterations - 1 - y, x] = rez;
        _arrPvc[_iterations - 1 - y, x] = StochasticProperties.PhaseVolumeContractionSpeed(lyap.Result);

        _progress.Iterate();
    }

    private void MakeGradient(int maxLeCount)
    {
        double maxCoeff = 1d;
        int arrLength = maxLeCount;
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
                    if (_arr[y, x] == -1)
                    {
                        continue;
                    }

                    if (_arrPvc[y, x] < mins[i] && _arr[y, x] == i)
                    {
                        mins[i] = _arrPvc[y, x];
                    }

                    if (_arrPvc[y, x] > maxs[i] && _arr[y, x] == i)
                    {
                        maxs[i] = _arrPvc[y, x];
                    }
                }
            }
        }

        for (int i = 0; i < arrLength; i++)
        {
            double amplitude = maxs[i] - mins[i];
            coeffs[i] = amplitude == 0 ? 0.5 : maxCoeff / amplitude;
        }

        for (int x = 0; x < _iterations; x++)
        {
            for (int y = 0; y < _iterations; y++)
            {
                if (_arr[y, x] == -1)
                {
                    continue;
                }

                int lec = (int)_arr[y, x];

                var addition = (_arrPvc[y, x] - mins[lec]) * coeffs[lec];

                if (addition == 0)
                {
                    addition = 1;
                }

                _arr[y, x] += addition * 0.95;
            }
        }
    }
}

public class LeSpecColormap : IColormap
{
    private readonly Color[] fullPalette =
    {
        Color.White,
        Color.ForestGreen,
        Color.DodgerBlue,
        Color.Orange,
        Color.Fuchsia,
    };

    private readonly Color[] rgb;

    public LeSpecColormap(int minLeIndex, int positiveLeCount)
    {
        Color[] tmp = fullPalette.Skip(minLeIndex + 1).Take(positiveLeCount + 1 - minLeIndex).ToArray();

        rgb = new Color[256];
        double coeff = (double)rgb.Length / tmp.Length;
        double vMin = 0.2;
        double vMax = 0.9;

        double vCoeff = (vMax - vMin) / coeff;

        int counter = 0;

        if (minLeIndex == -1)
        {
            for (int i = 0; i < Math.Round(coeff); i++)
            {
                rgb[i] = Color.White;
            }

            counter = 1;
        }

        for (int i = counter; i < tmp.Length; i++)
        {
            var hsv = ColorToHSV(tmp[i]);

            for (int j = 0; j < Math.Round(coeff); j++)
            {
                rgb[Math.Min((i * (int)Math.Round(coeff)) + j + 1, 255)] = ColorFromHSV(hsv.hue, hsv.saturation, vMin + j * vCoeff);
            }
        }
    }

    public string Name => "LeSpec";

    public (byte r, byte g, byte b) GetRGB(byte value)
    {
        Color col = rgb[value];
        return (col.R, col.G, col.B);
    }

    private static (double hue, double saturation, double value) ColorToHSV(Color color)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        double hue = color.GetHue();
        double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
        double value = max / 255d;

        return (hue, saturation, value);
    }

    private static Color ColorFromHSV(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }
}
