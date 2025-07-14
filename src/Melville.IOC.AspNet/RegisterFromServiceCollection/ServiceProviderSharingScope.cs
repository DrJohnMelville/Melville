using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class ServiceProviderSharingScope(IIocService outer) :
    CombinedScope(outer),
    IServiceProvider, ISupportRequiredService, IServiceScope, IServiceScopeFactory,
    IServiceProviderIsService
{
    /// <inheritdoc />
    protected override IScope WrapScope(IScope inner) =>
        new ServiceProviderScopeAdapter(inner, this);

    public object? GetService(Type serviceType) => this.Get(serviceType);

    public object GetRequiredService(Type serviceType) =>
        GetService(serviceType) ?? 
        throw new InvalidOperationException(
            $"Service of type {serviceType} could not be constructed");

    public IServiceProvider ServiceProvider => this;
    public IServiceScope CreateScope() => 
        new ServiceProviderSharingScope(ParentScope);

    public bool IsService(Type serviceType) => this.CanGet(serviceType);
}