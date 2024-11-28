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

    public bool CanGet(IBindingRequest request)
    {
        TrySetScopeValue(request);
        return ParentScope.CanGet(request);
    }

    public object? Get(IBindingRequest request)
    {
        TrySetScopeValue(request); return ParentScope.Get(request);
    }

    private void TrySetScopeValue(IBindingRequest request)
    {
        if (IsAChildOfThisScope(request.IocService)) return;
        request.IocService = this;
    }

    private bool IsAChildOfThisScope(IIocService scope) => 
        scope.ScopeList().Contains(this);

    public IIocDebugger Debugger => ParentScope.Debugger;
}

public interface IScope
{
    bool TryGetValue(IActivationStrategy source, [NotNullWhen(true)] out object? result);
    void SetScopeValue(IActivationStrategy source, object? value);
}
    
public sealed class SharingScopeContainer : GenericScope, IScope
{
    public SharingScopeContainer(IIocService parentScope) : base(parentScope)
    {
    }

    private readonly Dictionary<IActivationStrategy, object?> scopeItems =
        new Dictionary<IActivationStrategy, object?>();
        
    public bool TryGetValue(IActivationStrategy source, [NotNullWhen(true)] out object? value) =>
        scopeItems.TryGetValue(source, out value!) ||
        ((ParentScope as IScope)?.TryGetValue(source, out value!) ?? false);

    public void SetScopeValue(IActivationStrategy source, object? value) => scopeItems.Add(source, value);
}