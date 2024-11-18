using System;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class ServiceProviderAdapter : IServiceProvider, ISupportRequiredService, IServiceScope, 
    IServiceScopeFactory
{
    private IIocService inner;

    public ServiceProviderAdapter(IIocService inner)
    {
        this.inner = inner;
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
}