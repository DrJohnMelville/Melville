using System;
using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;


namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class MelvilleServiceProviderFactory: IServiceProviderFactory<IocContainer>
{
    private readonly bool allowRootDisposables;
    private readonly Action<IBindableIocService>? setupRegistration;

    public MelvilleServiceProviderFactory(
        bool allowRootDisposables, Action<IBindableIocService>? setupRegistration = null)
    {
        this.allowRootDisposables = allowRootDisposables;
        this.setupRegistration = setupRegistration;
    }

    public IocContainer CreateBuilder(IServiceCollection services)
    {
        var ret = new IocContainer();
        RegisterServiceCollectionWithContainer.BindServiceCollection(ret, services);
        setupRegistration?.Invoke(ret);
        return ret;
    }

    public IServiceProvider CreateServiceProvider(IocContainer containerBuilder)
    {
        containerBuilder.AllowDisposablesInGlobalScope = allowRootDisposables;
        var adapter = new ServiceProviderAdapter(containerBuilder);
        containerBuilder.BindIfMNeeded<IServiceScopeFactory>().And<IServiceProvider>()
            .ToConstant(adapter).DisposeIfInsideScope();
        // The outermost scope never gets disposed, but this is ok because it is supposed to last for
        // the entire program.
        return adapter;
    }
}