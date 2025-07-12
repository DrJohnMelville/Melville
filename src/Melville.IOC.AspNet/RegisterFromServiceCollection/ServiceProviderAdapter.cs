using System;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.Debuggers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public static class ServiceProviderAdaptorFactory
{
    public static IServiceProvider CreateServiceProvider(this IocContainer ioc, bool allDisposablesInRoot)
    {
        ioc.AllowDisposablesInGlobalScope = allDisposablesInRoot;
        ioc.AddTypeResolutionPolicyToEnd(new FailRequestPolicy());
        //var adapter = new ServiceProviderAdapter(ioc);

        //We are binding these all to null specifically because the special scope
        ioc.Bind<IServiceProvider>().To<FakeScopeObject>().AsScoped();
        ioc.Bind<ISupportRequiredService>().To<FakeScopeObject>().AsScoped();
        ioc.Bind<IServiceScope>().To<FakeScopeObject>().AsScoped();
        ioc.Bind<IServiceScopeFactory>().To<FakeScopeObject>().AsScoped();
        ioc.Bind<IServiceProviderIsService>().To<FakeScopeObject>().AsScoped();

        return new ServiceProviderSharingScope(ioc);

    }
    private class FakeScopeObject:
        IServiceProvider, ISupportRequiredService, IServiceScope, IServiceScopeFactory,
        IServiceProviderIsService
    {
        public FakeScopeObject()
        {
            throw new NotSupportedException("This placeholder type should never be instantiated");
        }

        public object? GetService(Type serviceType) => throw new NotImplementedException();
        public object GetRequiredService(Type serviceType) => throw new NotImplementedException();
        public void Dispose() => throw new NotImplementedException();
        public IServiceProvider ServiceProvider { get; }
        public IServiceScope CreateScope() => throw new NotImplementedException();
        public bool IsService(Type serviceType) => throw new NotImplementedException();
    }
}