using System;

namespace Melville.Generators.INPC.CodeWriters;

public class RunActionOnDispose: IDisposable
{
    private readonly Action disposeAction;

    public RunActionOnDispose(Action disposeAction)
    {
        this.disposeAction = disposeAction;
    }

    public void Dispose()
    {
        disposeAction();
    }
}