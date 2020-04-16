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
    
    public class DisposalRegister: IDisposable, IAsyncDisposable, IRegisterDispose
    {
        private readonly List<object> itemsToDispose = new List<object>();

        public void RegisterForDispose(object ret)
        {
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
            //We call distinct so that if the user accidentally got the class registered twice, ike because it
            // called registerWrapperForDisposal without a wrapper. it does not get disposed twice
            var finalDisposalList = Enumerable.Reverse(itemsToDispose).Distinct().ToList();
            
            // We clear the list before we dispose because this scope might get registered in itself, either
            // directly or indirectly.  so we want to be sure that we do no dispose recursively.
            itemsToDispose.Clear();
        
            return finalDisposalList;
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

    public class DisposableIocService: GenericScope, IDisposableIocService, IRegisterDispose
    {
        private readonly DisposalRegister register = new DisposalRegister();
        public DisposableIocService(IIocService parentScope) : base(parentScope)
        {
        }

        public ValueTask DisposeAsync() => register.DisposeAsync();
        public void Dispose() => register.Dispose();
        public void RegisterForDispose(object obj) => register.RegisterForDispose(obj);
    }
}