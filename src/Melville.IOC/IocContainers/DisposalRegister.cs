using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Melville.IOC.IocContainers;

public class DisposalRegister: IDisposable, IAsyncDisposable, IRegisterDispose
{
    private readonly ConcurrentStack<object> itemsToDispose = new();

    public void RegisterForDispose(object ret)
    {
        if (ret == this) return;
        if (itemsToDispose.Contains(ret)) return;
        itemsToDispose.Push(ret);
    }

    public async ValueTask DisposeAsync()
    {
        while (itemsToDispose.TryPop(out var item))
        {
            await DisposeSingleItemAsync(item);
        }
    }

    private static ValueTask DisposeSingleItemAsync(object item) =>
        item switch
        {
            IAsyncDisposable ad => ad.DisposeAsync(),
            IDisposable d => SynchronousDisposeAsValueTask(d),
            _ => new ValueTask()
        };

    private static ValueTask SynchronousDisposeAsValueTask(IDisposable d)
    {
        d.Dispose();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        while (itemsToDispose.TryPop(out var item))
        {
            DisposeSingleItem(item);
        }
    }

    private void DisposeSingleItem(object item)
    {
        switch (item)
        {
            case IDisposable d:
                d.Dispose();
                break;
            case IAsyncDisposable ad:
                _ = ad.DisposeAsync();
                break;
        }
    }

    public bool IsDisposalContainer => true;
}