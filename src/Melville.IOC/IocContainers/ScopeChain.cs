using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

public class ScopeChain(
    IBindingRequest prior, IScope items, IRegisterDispose disposer): 
    ForwardingRequest(prior), IScope
{
    /// <inheritdoc />
    public override IScope SharingScope => this;
    public override IRegisterDispose DisposeScope => disposer;

    #region IScopeImplementation
    /// <inheritdoc />
    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result) =>
        PriorScope().TryGetValue(source, key, out result)||
        items.TryGetValue(source, key, out result);

    private IScope PriorScope() => base.SharingScope;

    /// <inheritdoc />
    public bool TrySetValue(IBindingRequest source, object? value, IActivationStrategy key)
    {
        if (PriorScope().TrySetValue(source, value, key)) return true;
        items.TrySetValue(source, value, key);
        return true;
    }

    #endregion
}