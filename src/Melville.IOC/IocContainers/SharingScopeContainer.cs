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


    public IRegisterDispose DefaultDisposeRegistration
    {
        get => ParentScope.DefaultDisposeRegistration;
        set => ParentScope.DefaultDisposeRegistration = value;
    }

    public virtual bool CanGet(IBindingRequest request) => 
        ParentScope.CanGet(request);

    public virtual object? Get(IBindingRequest request) => 
        ParentScope.Get(request);

    public IIocDebugger Debugger => ParentScope.Debugger;
}

public interface IScope
{
    bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result);
    void SetScopeValue(IBindingRequest source, object? value, IActivationStrategy key);
}
    
public class SharingScopeContainer(IIocService parentScope) : 
    GenericScope(parentScope), IScope
{
    private readonly Dictionary<IActivationStrategy, object?> scopeItems = new();
        
    public virtual bool TryGetValue(IBindingRequest source, IActivationStrategy key,
        [NotNullWhen(true)] out object? value) =>
        scopeItems.TryGetValue(key, out value);

    public void SetScopeValue(IBindingRequest source, object? value, IActivationStrategy key) => 
        scopeItems.Add(key, value);
}