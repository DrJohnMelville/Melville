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
        base.Create(new CreateSingletonRequest(bindingRequest));

    public static IActivationStrategy EnsureSingleton(IActivationStrategy inner) =>
        inner.SharingScope() == IocContainers.SharingScope.Singleton
            ? inner
            : new SingletonActivationStrategy(inner);
}

public class CreateSingletonRequest(IBindingRequest parent) : ForwardingRequest(parent), IScope
{
    public override IScope SharingScope => this;

    /// <inheritdoc />
    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result)
    {
        // someone prior to us may have an AllowScopedInSingleton that lets us read through the
        // singleton barrier.  This class will complain about it by returning false, but the captured
        // value is propogated in Result so AllowScopedSingleton can restore the true response.
        base.SharingScope.TryGetValue(source, key, out result);
        return false;
    }

    /// <inheritdoc />
    public bool TrySetValue(IBindingRequest source, IActivationStrategy key, object? value)
    {
        // no one gets to write through the static barrier.
        return false;
    }
}

