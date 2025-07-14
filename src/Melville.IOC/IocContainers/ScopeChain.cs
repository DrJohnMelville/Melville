using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

public class ScopeChain(
    IBindingRequest prior, IIocService parentIoc, IScope items, IRegisterDispose disposer): 
    ForwardingRequest(prior), IScope
{
    public override IScope SharingScope => this;
    public override IRegisterDispose DisposeScope => disposer;
    public override IIocService IocService => parentIoc;

    private IScope PriorScope() => base.SharingScope;

    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result) =>
        PriorScope().TryGetValue(source, key, out result)||
        items.TryGetValue(source, key, out result);

    public bool TrySetValue(IBindingRequest source, IActivationStrategy key, object? value) => 
        PriorScope().TrySetValue(source, key, value) || 
        items.TrySetValue(source, key, value);
}