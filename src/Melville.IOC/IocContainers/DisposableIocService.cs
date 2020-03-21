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

    public enum DisposalState
    {
        DisposalRequired = 0,
        DisposalDone = 1,
        DisposeOptional = 2
    };

    public class DisposableIocService: IDisposableIocService
    {
        private readonly List<object> itemsToDispose = new List<object>();
        public DisposableIocService(IIocService parentScope)
        {
            ParentScope = parentScope;
        }

        public IIocService ParentScope { get; }

        public bool CanGet(IBindingRequest request) => ParentScope.CanGet(request);
        
        public (object? Result, DisposalState DisposalState) Get(IBindingRequest request)
        {
            request.IocService = this;
            var (ret, oldDisposalState) = ParentScope.Get(request);
            if (ElligibleForDispose(oldDisposalState, ret))
            {
                itemsToDispose.Add(ret);
            }

            return (ret, DisposalState.DisposalDone);
        }

        private static bool ElligibleForDispose(DisposalState oldDisposalState, [NotNullWhen(true)]object? ret) => 
            oldDisposalState != DisposalState.DisposalDone && IsDisposableItem(ret);

        private static bool IsDisposableItem([NotNullWhen(true)]object? ret) =>
            ret is IDisposable || ret is IAsyncDisposable;

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