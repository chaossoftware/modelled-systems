﻿using ChaosSoft.Core.IO;
using ChaosSoft.NumericalMethods.Equations;
using System.Drawing;
using System.IO;
using System.Text;

namespace ModelledSystems.Routines;

internal class SystemOut : Routine
{
    private readonly long _totalIterations;
    private readonly int _eqN;
    private readonly double _eqStep;
    private readonly double[][] outArray;
    private readonly SystemBase equations;
    private SolverBase solver;
    private readonly bool _binaryOutput;

    public SystemOut(string outDir, SystemCfg sysParams, bool binaryOutput) : base (outDir, sysParams)
    {
        _binaryOutput = binaryOutput;
        _eqStep = SysConfig.Solver.Dt;

        equations = GetSystemEquations(SysConfig.ParamsValues);
        _eqN = equations.Count;
        
        _totalIterations = (long)(SysConfig.Solver.ModellingTime / _eqStep) + 1;

        outArray = new double[_eqN][];
        
        for (int i = 0; i < outArray.Length; i++)
        {
            outArray[i] = new double[_totalIterations];
        }

        solver = GetSolver(equations);
    }

    public override void Run()
    {
        for (int i = 0; i < _totalIterations; i++)
        {
            solver.NexStep();

            for (int k = 0; k < _eqN; k++)
            {
                outArray[k][i] = solver.Solution[0, k];
            }
        }

        WriteResults();
    }

    private void WriteResults()
    {
        StringBuilder output = new StringBuilder();

        string fileNameStart = Path.Combine(OutDir, equations.ToFileName() + $"_st={_eqStep:0.###}");

        double[] xt = new double[_totalIterations];
        double[] yt = new double[_totalIterations];
        double[] zt = new double[_totalIterations];

        double t = 0;

        for (int cnt = 0; cnt < _totalIterations; cnt++)
        {
            output.AppendFormat("{0:F5}", t);

            for (int k = 0; k < _eqN; k++)
            {
                output.AppendFormat("\t{0:F15}", outArray[k][cnt]);
            }

            output.AppendLine();

            xt[cnt] = outArray[0][cnt];
            yt[cnt] = _eqN > 1 ? outArray[1][cnt] : 1;
            zt[cnt] = _eqN > 2 ? outArray[2][cnt] : 1;

            t += _eqStep;
        }

        if(_eqN > 1)
        {
            var plt = GetPlot("x", "y");
            plt.AddScatterPoints(xt, yt, Color.Blue, 1);
            plt.SaveFig(Path.Combine(OutDir, SysConfig.Name + "_attractor.png"));
        }
        
        if (_binaryOutput)
        {
            DataWriter.CreateBytesDataFile(fileNameStart + ".dat", outArray);
        }
        else
        {
            DataWriter.CreateDataFile(fileNameStart + ".dat", output.ToString());
        }

        Model3D.Create3daModelFile(fileNameStart + ".3da", xt, yt, zt);
        Sound.CreateWavFile(fileNameStart + ".wav", xt);
    }
}
