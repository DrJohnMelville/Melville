using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Melville.IOC.IocContainers
{
    public interface IDisposableIocService : IIocService, IAsyncDisposable, IDisposable
    { 
    }

    public interface IRegisterDispose
    {
        void RegisterForDispose(object obj);
    }
    
    public class DisposableIocService: GenericScope, IDisposableIocService, IRegisterDispose
    {
        public DisposableIocService(IIocService parentScope) : base(parentScope)
        {
        }

        private readonly List<object> itemsToDispose = new List<object>();

        public void RegisterForDispose(object? ret)
        {
            itemsToDispose.Add(ret);
        }

        // private static bool ElligibleForDispose(DisposalState oldDisposalState, [NotNullWhen(true)]object? ret) => 
        //     oldDisposalState != DisposalState.DisposalDone && IsDisposableItem(ret);
        //
        // private static bool IsDisposableItem([NotNullWhen(true)]object? ret) =>
        //     ret is IDisposable || ret is IAsyncDisposable;

        public async ValueTask DisposeAsync()
        {
            //We dispose in reverse order.  Since most objects are created after their dependencies that means
            // that most objects will be disposed before their dependencies are disposed.
            foreach (var item in Enumerable.Reverse(itemsToDispose))
            {
                await DisposeSingleItem(item);
            }
            itemsToDispose.Clear();
        }

        private static async Task DisposeSingleItem(object item)
        {
            switch (item)
            {
                case IAsyncDisposable ad:
                    await ad.DisposeAsync();
                    break;
                case IDisposable d:
                    d.Dispose();
                    break;
            }
        }

        public void Dispose()
        {
            // if we are disposed of without an async context, at least everything will get disposed of.
            // but the async disposal may continue after this function returns.  This is unfortunate but
            // it is the best we can do in the context
            GC.KeepAlive(DisposeAsync());
        }
    }
}