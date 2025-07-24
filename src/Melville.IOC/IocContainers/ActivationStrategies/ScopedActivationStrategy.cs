using System.Collections.Generic;
using System.Linq;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public class ScopedActivationStrategy : ForwardingActivationStrategy
{
    public ScopedActivationStrategy(IActivationStrategy innerActivationStrategy): base(innerActivationStrategy)
    {
        if (innerActivationStrategy.SharingScope() != IocContainers.SharingScope.Transient)
        {
            throw new IocException("Bindings may only specify at most one lifetime.");
        }
    }

    public override SharingScope SharingScope() => IocContainers.SharingScope.Scoped;
    
    public override object? Create(IBindingRequest bindingRequest)
    {
        if (bindingRequest.SharingScope.TryGetValue(bindingRequest, this, out var ret)) return ret;
        var value = base.Create(bindingRequest);
        if (!bindingRequest.SharingScope.TrySetValue(bindingRequest, this, value))
            throw new IocException("Attempted to create a scoped value outside of a scope.");
        return value;
    }
}