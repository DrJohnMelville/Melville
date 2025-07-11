using System;
using Melville.IOC.IocContainers;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class SlimIocToServiceProviderAdapter(IIocService container): IServiceProvider
{
    public object? GetService(Type serviceType) => container.Get(serviceType);
}