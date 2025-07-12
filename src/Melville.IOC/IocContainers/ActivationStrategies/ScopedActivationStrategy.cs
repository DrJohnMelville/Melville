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
    
    private IScope Scope(IBindingRequest req) => 
        ParentScopes(req).FirstOrDefault()??
        throw new IocException($"Attempted to create a scoped {req.DesiredType.Name} outside of a scope.");

    private static IEnumerable<IScope> ParentScopes(IBindingRequest req) => 
        req.IocService.ScopeList().OfType<IScope>();

    public override object? Create(IBindingRequest bindingRequest)
    {
        var bag = bindingRequest.IocService.ScopeList();
        var parentScopes = ParentScopes(bindingRequest);
        foreach (var parentScope in parentScopes)
        {
            if (parentScope.TryGetValue(bindingRequest, out var ret)) return ret;
        }

        return RecordScopedValue(bindingRequest, base.Create(bindingRequest));
    }

    private object? 
        RecordScopedValue(IBindingRequest bindingRequest, in object? create)
    {
        Scope(bindingRequest).SetScopeValue(bindingRequest, create);
        return create;
    }
}