using System;
using System.Linq;
using System.Threading.Tasks;
using ChaosSoft.Core.DataUtils;
using ChaosSoft.NumericalMethods.Ode;
using ChaosSoft.NumericalMethods.Lyapunov;
using ScottPlot;

namespace ModelledSystems.Routines;

internal sealed class LeFractal : Routine
{
    private readonly int _paramIndex;
    private readonly string _sequence;
    private readonly double[,] _arr;

    private readonly TaskProgress _progress;
    private readonly int _iterations;
    private readonly SysParamCfg _parameter;
    private readonly double _step;

    public LeFractal(string outDir, SystemCfg sysConfig, int paramIndex, int iterations, string sequence) 
        : base(outDir, sysConfig)
    {
        _paramIndex = paramIndex;
        _sequence = sequence;

        _parameter = SysConfig.Params[paramIndex];
        _iterations = iterations;
        _step = (_parameter.To - _parameter.From) / _iterations;
        _progress = new TaskProgress(_iterations * _iterations);

        _arr = new double[_iterations, _iterations];
        Matrix.FillWith(_arr, -1);
    }

    public override void Run()
    {
        Parallel.For(0, _iterations * _iterations, z => Func(z));
        GetImage();
    }

    private void GetImage()
    {
        Plot fractalPlot = GetPlot(_parameter.Name, _parameter.Name);
        
        //int maxPositiveLeIndex = (int)Matrixes.Max(_arr);
        //int minLeIndex = (int)Matrixes.Min(_arr);
        MakeGradient();

        var hm = fractalPlot.AddHeatmap(_arr, ScottPlot.Drawing.Colormap.Topo, lockScales: false);
        //var cb = plt.AddColorbar(hm);
        hm.Smooth = true;
        fractalPlot.Margins(0, 0);

        //double[] ticks = Arrays.GenerateUniformArray(maxPositiveLeIndex + 1, minLeIndex, 1d);

        //cb.SetTicks(
        //    ticks,
        //    ticks.Select(t => t.ToString()).ToArray(), 
        //    min: minLeIndex, 
        //    max: maxPositiveLeIndex);
        double[] positions = new double[] { 0, _iterations };
        string[] labels = new string[] { _parameter.From.ToString(), _parameter.To.ToString() };

        fractalPlot.XTicks(positions, labels);
        fractalPlot.YTicks(positions, labels);

        SavePlot(fractalPlot, FileNameBase + "_lyap_fractal.png");
    }

    public void Func(int z)
    {
        int x = z / _iterations;
        int y = z % _iterations;

        double firstValue = _parameter.From + x * _step;
        double secondValue = _parameter.From + y * _step;

        double[] vars = SysConfig.ParamsValues.ToArray();

        IOdeSys equations = GetSystemEquations(vars);
        double eqStep = SysConfig.Solver.Dt;
        long totIter = (long)(SysConfig.Solver.ModellingTime / eqStep);

        OdeSolverBase solver = SolverFactory.Get(SysConfig.Solver.Type, equations, eqStep);
        OdeSolverBase solverCopy = SolverFactory.Get(SysConfig.Solver.Type, equations, eqStep);
        solver.SetInitialConditions(0, SysConfig.InitialConditions);
        solverCopy.SetInitialConditions(0, SysConfig.InitialConditions);

        LleBenettin benettin = new(solver, solverCopy, totIter);
        benettin.MakeInitialConditionsDifference();

        for (int i = 0; i < totIter; i++)
        {
            double val = _sequence.ToLowerInvariant()[i % _sequence.Length] == 'a' ? firstValue : secondValue;
            //TODO determine vars

            vars[_paramIndex] = val;
            (equations as IHasParameters).SetParameters(vars);
            benettin.MakeIteration();
        }

        _arr[_iterations - 1 - y, x] = benettin.Result;
        _progress.Iterate();
    }

    private void MakeGradient()
    {
        double min = double.MaxValue;
        double max = double.MinValue;

        for (int x = 0; x < _iterations; x++)
        {
            for (int y = 0; y < _iterations; y++)
            {
                if (double.IsInfinity(_arr[y, x]))
                {
                    continue;
                }

                if (_arr[y, x] < min)
                {
                    min = _arr[y, x];
                }

                if (_arr[y, x] > max)
                {
                    max = _arr[y, x];
                }
            }
        }

        double absMin = Math.Abs(min);
        double coeff = absMin > max ? absMin / max : max / absMin;
        double posCoeff = absMin > max ? 1 : max / absMin;
        double negCoeff = max > absMin ? 1 : absMin / max;

        for (int x = 0; x < _iterations; x++)
        {
            for (int y = 0; y < _iterations; y++)
            {
                if (double.IsPositiveInfinity(_arr[y, x]))
                {
                    _arr[y, x] = max / posCoeff;
                } 
                else if (double.IsNegativeInfinity(_arr[y, x]))
                {
                    _arr[y, x] = min / negCoeff;
                }
                else if (_arr[y, x] == -1)
                {
                    _arr[y, x] = max / posCoeff;
                }
                else if (_arr[y, x] > 0)
                {
                    _arr[y, x] /= posCoeff;
                }
                else if (_arr[y, x] < 0)
                {
                    _arr[y, x] /= negCoeff;
                }
            }
        }

    }

    /*
     * For shades, multiply each component by 1/4, 1/2, 3/4, etc., of its previous value. 
     * The smaller the factor, the darker the shade.
     *
     * For tints, calculate (255 - previous value), multiply that by 1/4, 1/2, 3/4, etc. (the greater the factor, the lighter the tint), 
     * and add that to the previous value (assuming each.component is a 8-bit integer).
    */
}
