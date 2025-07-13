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
        var ret = new ServiceProviderSharingScope(ioc);
        ioc.Bind<IServiceProvider>().
            And<ISupportRequiredService>().
            And<IServiceScope>().
            And<IServiceScopeFactory>().
            And<IServiceProviderIsService>().ToMethod(()=>ret)
            .AsScoped().AllowScopeInsideSingleton();
        return ret;
    }
}