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

public class CreateSingletonRequest(IBindingRequest parent) : ForwardingRequest(parent)
{
    public override CreateSingletonRequest? SingletonRequestParent => this;

    public void TryCreateScopedChild(IBindingRequest childRequest)
    {
        throw new IocException(
            $"Cannot create a scoped {childRequest.DesiredType.PrettyName()} inside of a singleton {DesiredType.PrettyName()}");
    }
}

public class AllowScopedInsideSingletonActivationStrategy(
    IActivationStrategy innerActivationStrategy) : ForwardingActivationStrategy(innerActivationStrategy)
{
    public override object? Create(IBindingRequest bindingRequest) => 
        base.Create(
            RemoveSingletonParentRequest.StripOuterSingletonScope(bindingRequest));
}

public class RemoveSingletonParentRequest(IBindingRequest parent) : ForwardingRequest(parent)
{
    public override CreateSingletonRequest? SingletonRequestParent => null;

    public static IBindingRequest StripOuterSingletonScope(IBindingRequest request) =>
        request.SingletonRequestParent is null ? request : new RemoveSingletonParentRequest(request);

}