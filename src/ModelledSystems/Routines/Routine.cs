﻿using System;
using System.Drawing;
using ChaosSoft.Core.NumericalMethods.Equations;
using ModelledSystems.Equations;
using ModelledSystems.Equations.Linearized;

namespace ModelledSystems.Routines;

abstract class Routine
{
    protected Routine(string outDir, SystemParameters parameters)
    {
        OutDir = outDir;
        SysParameters = parameters;
    }

    protected string OutDir { get; set; }

    protected SystemParameters SysParameters { get; set; }

    public Size Size { get; set; }

    public abstract void Run();

    protected SystemBase GetSystemEquations(double[] vars)
    {
        SystemBase eq;
        switch (SysParameters.SystemName.ToLower())
        {
            case "henon":
                eq = new HenonMap();
                break;
            case "henon_generalized":
                eq = new GeneralizedHenonMap();
                break;
            case "logistic":
                eq = new LogisticMap();
                break;
            case "tinkerbell":
                eq = new TinkerbellMap();
                break;
            case "lorenz":
                eq = new LorenzAttractor();
                break;
            case "rossler":
                eq = new Rossler();
                break;
            case "thomas":
                eq = new ThomasAttractor();
                break;
            case "halvorsen":
                eq = new HalvorsenAttractor();
                break;
            case "qi_chen":
                eq = new QiChenAttractor();
                break;
            case "chua":
                eq = new ChuaCircuit();
                break;
            case "stankevich":
                eq = new Stankevich();
                break;
            case "charo":
                eq = new CharoAttractor();
                break;
            case "henon_helies":
                eq = new HenonHeiles();
                break;
            case "anischenko_nikolaev":
                eq = new AnishchenkoNikolaev();
                break;
            default:
                throw new ArgumentException($"No such system: {SysParameters.SystemName}");
        }

        eq.SetParameters(vars);
        return eq;
    }

    protected SystemBase GetLinearizedSystemEquations(double[] vars)
    {
        SystemBase eq;
        switch (SysParameters.SystemName.ToLower())
        {
            case "lorenz":
                eq = new LorenzLinearized();
                break;
            case "rossler":
                eq = new RosslerLinearized();
                break;
            case "henon":
                eq = new HenonLinearized();
                break;
            case "henon_generalized":
                eq = new GeneralizedHenonLinearized();
                break;
            case "logistic":
                eq = new LogisticLinearized();
                break;
            case "tinkerbell":
                eq = new TinkerbellLinearized();
                break;
            default:
                throw new ArgumentException($"No such system: {SysParameters.SystemName}");
        }

        eq.SetParameters(vars);
        return eq;
    }

    public SolverBase GetSolver(string name, SystemBase eq, double step)
    {
        switch(name.ToLowerInvariant())
        {
            case "discrete":
                return new DiscreteSolver(eq, step);
            case "rk5":
                return new RK5(eq, step);
            default:
                return new RK4(eq, step);
        }
    }

    public Type GetSolverType(string name)
    {
        switch (name.ToLowerInvariant())
        {
            case "discrete":
                return typeof(DiscreteSolver);
            case "rk5":
                return typeof(RK5);
            default:
                return typeof(RK4);
        }
    }
}
