using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public partial class ServiceProviderSharingScope(IIocService outer) :
    SharingScopeContainer(outer),
    IServiceProvider, ISupportRequiredService, IServiceScope, IServiceScopeFactory,
    IServiceProviderIsService
{
    #region

    public override bool CanGet(IBindingRequest request) => 
        base.CanGet(ChangeDisposeRegistration.TryDisposeChange(request, disposeContainer));

    public override object? Get(IBindingRequest request) => 
        base.Get(ChangeDisposeRegistration.TryDisposeChange(request, disposeContainer));

    #endregion

    #region Add this, with all its interfaces to the scope
    public override bool TryGetValue(IBindingRequest source, IActivationStrategy key,
        [NotNullWhen(true)] out object? value)
    {
        if (RequestingATypeThisImplements(source.DesiredType))
        {
            value = this;
            return true;
        }
        return base.TryGetValue(source, key, out value);
    }

    private static bool RequestingATypeThisImplements(Type type)
    {
        return type == typeof(IServiceProvider) ||
               type == typeof(IServiceScope) ||
               type == typeof(IServiceProviderIsService) ||
               type == typeof(IServiceScopeFactory) ||
               type == typeof(ISupportRequiredService);
    }
    #endregion

    #region Disposal

    private readonly DisposalRegister disposeContainer = new();

    public void Dispose() => disposeContainer.Dispose();

    #endregion

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