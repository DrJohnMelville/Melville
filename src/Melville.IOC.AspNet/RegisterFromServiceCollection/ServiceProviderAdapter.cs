using System;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.Debuggers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public static class ServiceProviderAdaptorFactory
{
    public static IServiceProvider CreateServiceProvider(this IocContainer containerBuilder, bool allDisposablesInRoot)
    {
        containerBuilder.AllowDisposablesInGlobalScope = allDisposablesInRoot;
        containerBuilder.AddTypeResolutionPolicyToEnd(new FailRequestPolicy());
        var adapter = new ServiceProviderAdapter(containerBuilder);
        containerBuilder.BindIfNeeded<IServiceScopeFactory>()
            .And<IServiceProvider>()
            .And<IServiceProviderIsService>()
            .And<IServiceScope>()
            .And<ISupportRequiredService>()
            .ToConstant(adapter).DisposeIfInsideScope();
        return adapter;
    }
}
public class ServiceProviderAdapter : 
    IServiceProvider, ISupportRequiredService, IServiceScope, IServiceScopeFactory,
    IServiceProviderIsService
{
    private IIocService inner;

    public ServiceProviderAdapter(IIocService inner)
    {
        this.inner = new ScopeWrapper(inner, this);
    }

    public object? GetService(Type serviceType)
    {
       return inner.Get(serviceType);
    }

    public object GetRequiredService(Type serviceType)
    {
        var request = new RootBindingRequest(serviceType, inner);
        var requiredService = inner.Get(request);
        if (request.IsCancelled || requiredService is null)
            throw new InvalidOperationException($"Service of type {serviceType} could not be constructed");
        return requiredService;
    }

    public void Dispose()
    {
        (inner as IDisposable)?.Dispose();
    }

    public IServiceProvider ServiceProvider => this;
    public IServiceScope CreateScope()
    {
        return new ServiceProviderAdapter(inner.CreateScope());
    }

    public bool IsService(Type serviceType)
    {
        return inner.CanGet(serviceType);
    }
}

public partial class ScopeWrapper (IIocService inner, ServiceProviderAdapter proxy) : IIocService
{
    public bool CanGet(IBindingRequest request)
    {
        return IsKnownType(request.DesiredType) || inner.CanGet(request);
    }

    private bool IsKnownType(Type type)
    {
        return type == typeof(IServiceProvider) || 
               type == typeof(IServiceScope) || 
               type == typeof(IServiceProviderIsService) || 
               type == typeof(IServiceScopeFactory) ||
               type == typeof(ISupportRequiredService);
    }

    public object? Get(IBindingRequest request) => 
        IsKnownType(request.DesiredType) ? proxy : inner.Get(request);

    public IIocService? ParentScope => inner;

    public bool AllowDisposablesInGlobalScope
    {
        get => inner.AllowDisposablesInGlobalScope;
        set => inner.AllowDisposablesInGlobalScope = value;
    }

    public IIocDebugger Debugger => inner.Debugger;
}