using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Melville.IOC.IocContainers;

public interface IDisposableIocService : IIocService, IAsyncDisposable, IDisposable
{ 
}

public interface IRegisterDispose
{
    void RegisterForDispose(object obj);
    bool SatisfiesDisposeRequirement { get; }
}
    
public class DisposalRegister: IDisposable, IAsyncDisposable, IRegisterDispose
{
    private readonly List<object> itemsToDispose = new List<object>();

    public void RegisterForDispose(object ret)
    {
        if (ret == this) return;
        if (itemsToDispose.Contains(ret)) return;
        itemsToDispose.Add(ret);
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var item in GenerateDisposalList())
        {
            await DisposeSingleItem(item);
        }
    }

    private List<object> GenerateDisposalList()
    {
        //We dispose in reverse order.  Since most objects are created after their dependencies that means
        // that most objects will be disposed before their dependencies are disposed.
        //
        var finalDisposalList = Enumerable.Reverse(itemsToDispose).ToList();
            
        // We clear the list before we dispose because this scope might get registered in itself, either
        // directly or indirectly.  so we want to be sure that we do no dispose recursively.
        itemsToDispose.Clear();
        
        return finalDisposalList;
    }

    private static ValueTask DisposeSingleItem(object item) =>
        item switch
        {
            IAsyncDisposable ad => ad.DisposeAsync(),
            IDisposable d => SynchronousDisposeAsValueTask(d),
            _ => new ValueTask()
        };

    private static ValueTask SynchronousDisposeAsValueTask(IDisposable d)
    {
        d.Dispose();
        return new ValueTask();
    }

    public void Dispose()
    {
        // if we are disposed of without an async context, at least everything will get disposed of.
        // but the async disposal may continue after this function returns.  This is unfortunate but
        // it is the best we can do in the context
        GC.KeepAlive(DisposeAsync());
    }

    public bool SatisfiesDisposeRequirement => true;
}

public class DisposableIocService: GenericScope, IDisposableIocService, IRegisterDispose
{
    private readonly DisposalRegister register = new DisposalRegister();
    public DisposableIocService(IIocService parentScope) : base(parentScope)
    {
    }

    public ValueTask DisposeAsync() => register.DisposeAsync();
    public void Dispose() => register.Dispose();
    public void RegisterForDispose(object obj)
    {
        if (obj == this) return;
        register.RegisterForDispose(obj);
    }

    public bool SatisfiesDisposeRequirement => register.SatisfiesDisposeRequirement;
}