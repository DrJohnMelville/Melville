using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public partial class ServiceProviderSharingScope(IIocService outer) :
    CombinedScope(outer),
    IServiceProvider, ISupportRequiredService, IServiceScope, IServiceScopeFactory,
    IServiceProviderIsService
{
    /// <inheritdoc />
    protected override IScope WrapScope(IScope inner) =>
        new ServiceProviderScopeAdapter(inner, this);


    #region Asp.Net overrides

    public object? GetService(Type serviceType) => this.Get(serviceType);

    public object GetRequiredService(Type serviceType) =>
        GetService(serviceType) ?? 
        throw new InvalidOperationException(
            $"Service of type {serviceType} could not be constructed");

    public IServiceProvider ServiceProvider => this;
    public IServiceScope CreateScope() => 
        new ServiceProviderSharingScope(ParentScope);

    public bool IsService(Type serviceType) => this.CanGet(serviceType);

    #endregion
}

public class ServiceProviderScopeAdapter(
    IScope inner, ServiceProviderSharingScope specialValue) : IScope
{
    /// <inheritdoc />
    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result)
    {
        if (inner.TryGetValue(source, key, out result))
            return true;
        if (RequestingATypeThisImplements(source.DesiredType))
        {
            result = specialValue;
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public bool TrySetValue(IBindingRequest source, object? value, IActivationStrategy key) => 
        inner.TrySetValue(source, value, key);

    private static bool RequestingATypeThisImplements(Type type)
    {
        return type == typeof(IServiceProvider) ||
               type == typeof(IServiceScope) ||
               type == typeof(IServiceProviderIsService) ||
               type == typeof(IServiceScopeFactory) ||
               type == typeof(ISupportRequiredService);
    }

}