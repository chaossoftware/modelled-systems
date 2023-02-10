using System;
using System.Runtime.CompilerServices;

namespace ModelledSystems.Routines;

internal class TaskProgress
{
    private readonly double _step;
    private int currentIteration;
    private int printedSymbols;

    public TaskProgress(int totalIterations)
    {
        printedSymbols = 0;
        currentIteration = 1;
        _step = totalIterations / (double)Console.BufferWidth;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Iterate()
    {
        if (currentIteration++ / _step > printedSymbols)
        {
            Console.Write("#");

            printedSymbols++;
        }
    }
}
