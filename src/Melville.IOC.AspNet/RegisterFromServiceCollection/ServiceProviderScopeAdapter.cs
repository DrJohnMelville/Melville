using System;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class ServiceProviderScopeAdapter(
    IScope inner, ServiceProviderSharingScope specialValue) : IScope
{
    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result)
    {
        if (inner.TryGetValue(source, key, out result)) return true;
        if (!RequestingATypeThisImplements(source.DesiredType)) return false;
        result = specialValue;
        return true;
    }

    public bool TrySetValue(IBindingRequest source, IActivationStrategy key, object? value) => 
        inner.TrySetValue(source, key, value);

    private static bool RequestingATypeThisImplements(Type type) =>
        type == typeof(IServiceProvider) ||
        type == typeof(IServiceScope) ||
        type == typeof(IServiceProviderIsService) ||
        type == typeof(IServiceScopeFactory) ||
        type == typeof(ISupportRequiredService);
}