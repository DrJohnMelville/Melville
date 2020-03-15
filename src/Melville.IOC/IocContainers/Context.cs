using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace Melville.IOC.IocContainers
{
    public interface IScope
    {
        bool TryGetValue(IActivationStrategy source, [NotNullWhen(true)] out object? result);
        void SetScopeValue(IActivationStrategy source, object? value);
    }

    public interface IDisposableIocService : IIocService, IAsyncDisposable, IDisposable
    { 
    }

    public sealed class Scope : IScope, IDisposableIocService, IIocService
    {
        private readonly IIocService innerService;
        
        public Scope(IIocService innerService)
        {
            this.innerService = innerService;
        }

        #region Dispose

        public void Dispose() => GC.KeepAlive(DisposeAsync());
       
        public ValueTask DisposeAsync()
        {
            // dispose of the nonasync disposables first
            foreach (var item in scopeItems.Values.OfType<IDisposable>())
            {
                item?.Dispose();
            }

            var asyncs = scopeItems.Values.OfType<IAsyncDisposable>().ToList();
            scopeItems.Clear();
            return asyncs.Count() == 0? new ValueTask() : new ValueTask(DisposeAllAsync(asyncs));
        }

        private async Task DisposeAllAsync(IEnumerable<IAsyncDisposable> asyncs)
        {
            foreach (var item in asyncs)
            {
                if (item != null)
                {
                    await item.DisposeAsync();
                }
            }
        }

        #endregion

        #region IIocService

        public bool CanGet(IBindingRequest request)
        {
            request.IocService = this;
            return innerService.CanGet(request);
        }

        object? IIocService.Get(IBindingRequest request)
        {
            request.IocService = this;
            return innerService.Get(request);
        }

        public IIocService GlobalScope => innerService.GlobalScope;
        #endregion

        #region IScope

        private readonly Dictionary<IActivationStrategy, object?> scopeItems =
            new Dictionary<IActivationStrategy, object?>();
        
        public bool TryGetValue(IActivationStrategy source, [NotNullWhen(true)] out object? value) =>
            scopeItems.TryGetValue(source, out value) ||
            ((innerService as IScope)?.TryGetValue(source, out value) ?? false);

        public void SetScopeValue(IActivationStrategy source, object? value) => scopeItems.Add(source, value);

        #endregion
    }
}