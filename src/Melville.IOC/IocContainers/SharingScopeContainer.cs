using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.Debuggers;


namespace Melville.IOC.IocContainers;

public abstract class GenericScope(IIocService parentScope) : IIocService
{
    public IIocService ParentScope { get; } = parentScope;


    public IRegisterDispose DefaultDisposeRegistration
    {
        get => ParentScope.DefaultDisposeRegistration;
        set => ParentScope.DefaultDisposeRegistration = value;
    }

    public virtual bool CanGet(IBindingRequest request) => 
        ParentScope.CanGet(WrapRequest(request));

    public virtual object? Get(IBindingRequest request) => 
        ParentScope.Get(WrapRequest(request));

    public IIocDebugger Debugger => ParentScope.Debugger;

    protected abstract IBindingRequest WrapRequest(IBindingRequest request);
}

public class SharingScopeContainer(IIocService parentScope) : 
    GenericScope(parentScope)
{
    private readonly ScopeRegistry scopeItems = new();

    /// <inheritdoc />
    protected override IBindingRequest WrapRequest(IBindingRequest request) =>
        new ScopeChain(request, scopeItems, request.DisposeScope);
}

public partial class CombinedScope(IIocService parent) : GenericScope(parent), IDisposableIocService
{
    private readonly ScopeRegistry scopeItems = new();
    private readonly DisposalRegister register = new();

    /// <inheritdoc />
    protected override IBindingRequest WrapRequest(IBindingRequest request) => 
        new ScopeChain(request, WrapScope(scopeItems), 
            ChangeDisposeRegistration.TryDisposeChange(request.DisposeScope, register));

    protected virtual IScope WrapScope(IScope inner) => inner;

    /// <inheritdoc />
    public void Dispose() => register.Dispose();

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}

public class ScopeRegistry: Dictionary<IActivationStrategy, object?>, IScope
{
    /// <inheritdoc />
    public bool TryGetValue(
        IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result) =>
        TryGetValue(key, out result);

    /// <inheritdoc />
    public bool TrySetValue(IBindingRequest source, object? value, IActivationStrategy key)
    {
        this[key] = value;
        return true;
    }
}