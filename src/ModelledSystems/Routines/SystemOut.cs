using ChaosSoft.Core;
using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Ode;
using ChaosSoft.NumericalMethods.Transform;
using ModelledSystems.Configuration;
using ScottPlot;
using System.Drawing;
using System.Text;

namespace ModelledSystems.Routines;

internal sealed class SystemOut : Routine
{
    private readonly long _iterations;
    private readonly double _dt;
    private readonly double[][] _output;
    private readonly IOdeSys _equations;
    private readonly OdeSolverBase _solver;
    private readonly bool _binaryOutput;

    public SystemOut(string outDir, SystemCfg sysParams, bool binaryOutput) : base (outDir, sysParams)
    {
        _binaryOutput = binaryOutput;
        _dt = sysParams.Solver.Dt;
        _equations = GetSystemEquations(sysParams.ParamsValues);
        _iterations = (long)(sysParams.Solver.ModellingTime / _dt) + 1;

        _output = new double[_equations.EqCount][];
        
        for (int i = 0; i < _equations.EqCount; i++)
        {
            _output[i] = new double[_iterations];
        }

        _solver = GetSolver(_equations);
    }

    public override void Run()
    {
        _solver.SetInitialConditions(0, SysConfig.InitialConditions);

        for (int i = 0; i < _iterations; i++)
        {
            _solver.NextStep();

            for (int k = 0; k < _equations.EqCount; k++)
            {
                _output[k][i] = _solver.Solution[k];
            }
        }

        WriteResults();
    }

    private void WriteResults()
    {
        string baseFilePath = GetBaseFilePath(_equations, _dt);

        StringBuilder output = new();

        double[] xt = new double[_iterations];
        double[] yt = new double[_iterations];
        double[] zt = new double[_iterations];

        double t = 0;
        int eqN = _equations.EqCount;

        for (int cnt = 0; cnt < _iterations; cnt++)
        {
            output.Append(NumFormat.Format(t, Constants.TimeOutput));

            for (int k = 0; k < eqN; k++)
            {
                output.Append(Constants.ColumnDelimiter)
                    .Append(NumFormat.Format(_output[k][cnt], Constants.LongOutput));
            }

            output.AppendLine();

            xt[cnt] = _output[0][cnt];
            yt[cnt] = eqN > 1 ? _output[1][cnt] : 0;
            zt[cnt] = eqN > 2 ? _output[2][cnt] : 0;

            t += _dt;
        }

        if (eqN > 1)
        {
            Plot attractorPlot = GetPlot("x", "y");

            if (_solver is DiscreteSolver)
            {
                attractorPlot.AddScatterPoints(xt, yt, Color.Blue, 0.5f);
            }
            else
            {
                attractorPlot.AddScatterLines(xt, yt, Color.Blue, 0.25f);
            }

            SavePlot(attractorPlot, baseFilePath + "_attractor.png");
        }

        if (_binaryOutput)
        {
            new BinaryDataFileWriter().WriteData(baseFilePath + ".bin", _output);
        }
        else
        {
            FileUtils.CreateDataFile(baseFilePath + ".dat", output.ToString());
        }

        Model3D.Create3daModelFile(baseFilePath + ".3da", xt, yt, zt);
        Sound.CreateWavFile(baseFilePath + ".wav", xt);
    }
}
