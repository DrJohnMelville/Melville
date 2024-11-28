using System;
using Melville.IOC.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.Extensions.DependencyInjection;


namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class MelvilleServiceProviderFactory(bool allowRootDisposables, Action<IBindableIocService>? setupRegistration = null)
    : IServiceProviderFactory<IocContainer>
{
    public IocContainer CreateBuilder(IServiceCollection services)
    {
        var ret = new IocContainer();
        ret.BindServiceCollection(services);
        setupRegistration?.Invoke(ret);
        return ret;
    }

    public IServiceProvider CreateServiceProvider(IocContainer containerBuilder)
    {
        containerBuilder.AllowDisposablesInGlobalScope = allowRootDisposables;
        containerBuilder.AddTypeResolutionPolicyToEnd(new FailRequestPolicy());
        var adapter = new ServiceProviderAdapter(containerBuilder);
        containerBuilder.BindIfNeeded<IServiceScopeFactory>()
            .And<IServiceProvider>()
            .And<IServiceProviderIsService>()
            .ToConstant(adapter).DisposeIfInsideScope();
        // The outermost scope never gets disposed, but this is ok because it is supposed to last for
        // the entire program.
        return adapter;
    }
}