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
            var context = ContextForSingletonCreation(bindingRequest);
            return base.Create(context);
    }
      
    private static IBindingRequest ContextForSingletonCreation(IBindingRequest bindingRequest) =>
        MustWrapContexxt(bindingRequest) ?
            new ChangeIocServiceRequest(bindingRequest,
                new SingleltonCreator(bindingRequest.IocService)):
            bindingRequest;

    private static bool MustWrapContexxt(IBindingRequest bindingRequest)
    {
        var scope = bindingRequest.IocService.ScopeList().OfType<IRegisterDispose>().FirstOrDefault();
        return scope switch
        {
            null => false,
            DisposableChildContainer  => false,
            _ => true
        };
    }

    public static IActivationStrategy EnsureSingleton(IActivationStrategy inner) =>
        inner.SharingScope() == IocContainers.SharingScope.Singleton
            ? inner
            : new SingletonActivationStrategy(inner);
}

public partial class SingleltonCreator : IIocService, IRegisterDispose, IScope
{
    [FromConstructor][DelegateTo] private readonly IIocService inner;
    public void RegisterForDispose(object obj)
    {
        if (!inner.AllowDisposablesInGlobalScope)
            throw new IocException("Attempted to create a Disposable object in singleton scope");
    }

    public bool SatisfiesDisposeRequirement => false;
    public bool TryGetValue(IBindingRequest source, [NotNullWhen(true)] out object? result)
    {
        throw new IocException("Attempted to create a scoped object in singleton scope");
    }

    public void SetScopeValue(IBindingRequest source, object? value)
    {
        throw new IocException("Attempted to create a scoped object in singleton scope");
    }
}