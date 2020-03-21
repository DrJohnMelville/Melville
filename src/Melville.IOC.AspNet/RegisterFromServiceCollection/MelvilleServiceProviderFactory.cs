using System;
using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;


namespace Melville.IOC.AspNet.RegisterFromServiceCollection
{
    public class MelvilleServiceProviderFactory: IServiceProviderFactory<IocContainer>
    {
        public IocContainer CreateBuilder(IServiceCollection services)
        {
            var ret = new IocContainer();
            RegisterServiceCollectionWithContainer.BindServiceCollection(ret, services);
            return ret;
        }

        public IServiceProvider CreateServiceProvider(IocContainer containerBuilder)
        {
            var adapter = new ServiceProviderAdapter(containerBuilder);
            containerBuilder.BindIfMNeeded<IServiceScopeFactory>().And<IServiceProvider>()
                .ToConstant(adapter).DisposeIfInsideScope();
              // The outermost scope never gets disposed, but this is ok because it is supposed to last for
              // the entire program.
            return adapter;
        }
    }

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
}