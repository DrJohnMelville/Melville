using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.Debuggers;


namespace Melville.IOC.IocContainers;

public class GenericScope: IIocService
{
    public GenericScope(IIocService parentScope)
    {
        ParentScope = parentScope;
    }

    public IIocService ParentScope { get; }

    public bool AllowDisposablesInGlobalScope
    {
        get => ParentScope.AllowDisposablesInGlobalScope;
        set => ParentScope.AllowDisposablesInGlobalScope = value;
    }

    public bool CanGet(IBindingRequest request) => 
        ParentScope.CanGet(request);

    public object? Get(IBindingRequest request) => 
        ParentScope.Get(request);

    public IIocDebugger Debugger => ParentScope.Debugger;
}

public interface IScope
{
    bool TryGetValue(IBindingRequest source, [NotNullWhen(true)] out object? result);
    void SetScopeValue(IBindingRequest source, object? value);
}
    
public class SharingScopeContainer(IIocService parentScope) : 
    GenericScope(parentScope), IScope
{
    private readonly Dictionary<Type, object?> scopeItems = new();
        
    public virtual bool TryGetValue(
        IBindingRequest source, [NotNullWhen(true)] out object? value) =>
        scopeItems.TryGetValue(source.DesiredType, out value);

    public void SetScopeValue(IBindingRequest source, object? value) => 
        scopeItems.Add(source.DesiredType, value);
}