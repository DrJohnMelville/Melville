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
        ioc.Bind<IServiceProvider>().ToMethod(ShouldNeverGetHere).AsScoped();
        ioc.Bind<ISupportRequiredService>().ToMethod(ShouldNeverGetHere).AsScoped();
        ioc.Bind<IServiceScope>().ToMethod(ShouldNeverGetHere).AsScoped();
        ioc.Bind<IServiceScopeFactory>().ToMethod(ShouldNeverGetHere).AsScoped();
        ioc.Bind<IServiceProviderIsService>().ToMethod(ShouldNeverGetHere).AsScoped();

        return new ServiceProviderSharingScope(ioc);

    }

    private static IServiceProvider ShouldNeverGetHere(IIocService arg1, IBindingRequest arg2)
    {
        throw new InvalidOperationException(
            $"{arg2.DesiredType} was requested outside of an ServiceProviderSharingScope");
    }
}