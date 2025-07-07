using System;
using System.Threading;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public class SingletonActivationStrategy : ForwardingActivationStrategy
{
    private IBindingRequest? lastRequst;

    private Lazy<object?> value;

    public SingletonActivationStrategy(IActivationStrategy innerActivationStrategyStrategy): base(innerActivationStrategyStrategy)
    { 
        if (innerActivationStrategyStrategy.SharingScope() != IocContainers.SharingScope.Transient)
        {
            throw new IocException("Bindings may only specify at most one lifetime.");
        }
        value = new Lazy<object?>(ComputeSingleValue, LazyThreadSafetyMode.ExecutionAndPublication);
    }
    public override SharingScope SharingScope() => IocContainers.SharingScope.Singleton;

    public override object? Create(IBindingRequest bindingRequest)
    {
        try
        {
            lastRequst = bindingRequest;
            return value.Value;
        }
        finally
        {
            lastRequst = null;
        }
    }
    private object? ComputeSingleValue() => ComputeSingleValue(lastRequst??
       throw new InvalidOperationException("Should have a lastRequest at this point"));

    private object? ComputeSingleValue(IBindingRequest bindingRequest)
    {
        return base.Create(new SingletonBindingRequest(bindingRequest));
    }

    public static IActivationStrategy EnsureSingleton(IActivationStrategy inner) =>
        inner.SharingScope() == IocContainers.SharingScope.Singleton
            ? inner
            : new SingletonActivationStrategy(inner);
}