using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using System.Drawing;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines;

internal class SystemOut : Routine
{
    private readonly long _iterations;
    private readonly int _eqN;
    private readonly double _dt;
    private readonly double[][] _systemOut;
    private readonly SystemBase _equations;
    private readonly SolverBase _solver;
    private readonly bool _binaryOutput;

    public SystemOut(string outDir, SystemCfg sysParams, bool binaryOutput) : base (outDir, sysParams)
    {
        _binaryOutput = binaryOutput;
        _dt = sysParams.Solver.Dt;
        _equations = GetSystemEquations(sysParams.ParamsValues);
        _eqN = _equations.Count;
        _iterations = (long)(sysParams.Solver.ModellingTime / _dt) + 1;
        _systemOut = new double[_eqN][];
        
        for (int i = 0; i < _systemOut.Length; i++)
        {
            _systemOut[i] = new double[_iterations];
        }

        _solver = GetSolver(_equations);
    }

    public override void Run()
    {
        for (int i = 0; i < _iterations; i++)
        {
            _solver.NexStep();

            for (int k = 0; k < _eqN; k++)
            {
                _systemOut[k][i] = _solver.Solution[0, k];
            }
        }

        WriteResults();
    }

    private void WriteResults()
    {
        StringBuilder output = new StringBuilder();

        string fileNameStart = Path.Combine(OutDir, _equations.ToFileName() + $"_st={_dt:0.###}");

        double[] xt = new double[_iterations];
        double[] yt = new double[_iterations];
        double[] zt = new double[_iterations];

        double t = 0;

        for (int cnt = 0; cnt < _iterations; cnt++)
        {
            output.AppendFormat("{0:F5}", t);

            for (int k = 0; k < _eqN; k++)
            {
                output.AppendFormat("\t{0:F15}", _systemOut[k][cnt]);
            }

            output.AppendLine();

            xt[cnt] = _systemOut[0][cnt];
            yt[cnt] = _eqN > 1 ? _systemOut[1][cnt] : 1;
            zt[cnt] = _eqN > 2 ? _systemOut[2][cnt] : 1;

            t += _dt;
        }

        if(_eqN > 1)
        {
            var plt = GetPlot("x", "y");
            plt.AddScatterPoints(xt, yt, Color.Blue, 1);
            plt.SaveFig(FileNameBase + "_attractor.png");
        }
        
        if (_binaryOutput)
        {
            DataWriter.CreateBytesDataFile(fileNameStart + ".dat", _systemOut);
        }
        else
        {
            DataWriter.CreateDataFile(fileNameStart + ".dat", output.ToString());
        }

        Model3D.Create3daModelFile(FileNameBase + ".3da", xt, yt, zt);
        Sound.CreateWavFile(FileNameBase + ".wav", xt);
    }
}
