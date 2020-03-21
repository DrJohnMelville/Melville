using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers
{
    public interface IScope
    {
        bool TryGetValue(IActivationStrategy source, [NotNullWhen(true)] out object? result);
        void SetScopeValue(IActivationStrategy source, object? value);
    }
    
    public sealed class SharingScopeContainer : IScope, IIocService
    {
        public IIocService ParentScope { get; }

        public SharingScopeContainer(IIocService parentScope)
        {
            ParentScope = parentScope;
        }
        
        #region IIocService

        public bool CanGet(IBindingRequest request)
        {
            request.IocService = this;
            return ParentScope.CanGet(request);
        }

        (object? Result, DisposalState DisposalState) IIocService.Get(IBindingRequest request)
        {
            request.IocService = this;
            return ParentScope.Get(request);
        }

        #endregion

        #region IScope

        private readonly Dictionary<IActivationStrategy, object?> scopeItems =
            new Dictionary<IActivationStrategy, object?>();
        
        public bool TryGetValue(IActivationStrategy source, [NotNullWhen(true)] out object? value) =>
            scopeItems.TryGetValue(source, out value) ||
            ((ParentScope as IScope)?.TryGetValue(source, out value) ?? false);

        public void SetScopeValue(IActivationStrategy source, object? value) => scopeItems.Add(source, value);

        #endregion
    }
}