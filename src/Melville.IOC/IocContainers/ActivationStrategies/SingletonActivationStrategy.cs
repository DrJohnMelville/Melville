using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ChildContainers;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public class SingletonActivationStrategy : ForwardingActivationStrategy
{
    private volatile object? value;
    private Lock? mutex = new();


    public SingletonActivationStrategy(IActivationStrategy innerActivationStrategyStrategy) : base(
        innerActivationStrategyStrategy)
    {
        if (innerActivationStrategyStrategy.SharingScope() != IocContainers.SharingScope.Transient)
        {
            throw new IocException("Bindings may only specify at most one lifetime.");
        }
    }

    public override SharingScope SharingScope() => IocContainers.SharingScope.Singleton;

    public override object? Create(IBindingRequest bindingRequest)
    {
        if (mutex is not { } capturedMutex) return value;
        lock (capturedMutex)
        {
            if (mutex is null) return value;
            mutex = null;
            value = ComputeSingleValue(bindingRequest);
        }
        return value;
    }


    private object? ComputeSingleValue(IBindingRequest bindingRequest) => 
        base.Create(ContextForSingletonCreation(bindingRequest));

    private static IBindingRequest ContextForSingletonCreation(IBindingRequest bindingRequest) =>
        DisposalScope(bindingRequest) is { } scopeParent
            ? NoScopesInsideSingleton(bindingRequest, scopeParent)
            : bindingRequest;

    private static ChangeIocServiceRequest NoScopesInsideSingleton(IBindingRequest bindingRequest, IRegisterDispose scopeParent)
    {
        return new ChangeIocServiceRequest(bindingRequest,
            new ForbidScopedInsideSingleton(bindingRequest.IocService,
                scopeParent));
    }

    private static IRegisterDispose? DisposalScope(IBindingRequest bindingRequest) =>
        bindingRequest.IocService
            .ScopeList()
            .OfType<IRegisterDispose>()
            .FirstOrDefault(x => x.SatisfiesDisposeRequirement);

    public static IActivationStrategy EnsureSingleton(IActivationStrategy inner) =>
        inner.SharingScope() == IocContainers.SharingScope.Singleton
            ? inner
            : new SingletonActivationStrategy(inner);
}

public partial class ForbidScopedInsideSingleton : IIocService, IRegisterDispose, IScope
{
    [FromConstructor] [DelegateTo] private readonly IIocService inner;
    [FromConstructor] private readonly IRegisterDispose overrideQuery;

#warning -- I think I nned two properties allow singletonsinside and mah be scoped inside of singleton
    public bool ShouldOverride(IBindingRequest br) =>
        overrideQuery.AllowSingletonInside(br.DesiredType);

    public void RegisterForDispose(object obj)
    {
        if (inner.AllowDisposablesInGlobalScope || obj is null ||
            overrideQuery.AllowSingletonInside(obj.GetType()))
            overrideQuery.RegisterForDispose(obj);
        else
            throw new IocException("Attempted to create a Disposable object in singleton scope");
    }

    public IIocService? ParentScope => inner;


    public bool SatisfiesDisposeRequirement => false;

    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result)
    {
        if (!ShouldOverride(source))
            throw new IocException("Attempted to create a scoped object in singleton scope");
        result = null;
        return false;
    }

    public void SetScopeValue(IBindingRequest source, object? value, IActivationStrategy key)
    {
        if (!ShouldOverride(source))
            throw new IocException("Attempted to create a scoped object in singleton scope");
    }

    public bool AllowSingletonInside(Type request) => false;
}