using System;
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
        try
        {
            return inner.Get(serviceType);
        }
        catch (IocException) // .Net provider does not throw when cannot create an object
        {
            return null;
        }
    }

    public object GetRequiredService(Type serviceType) => inner.Get(serviceType);

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