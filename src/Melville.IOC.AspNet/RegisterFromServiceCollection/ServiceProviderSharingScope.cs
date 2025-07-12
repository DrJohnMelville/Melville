using Melville.IOC.IocContainers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public partial class ServiceProviderSharingScope(IIocService outer) :
    SharingScopeContainer(outer), IRegisterDispose,
    IServiceProvider, ISupportRequiredService, IServiceScope, IServiceScopeFactory,
    IServiceProviderIsService
{
    #region Add this, with all its overrides, to the scope
    public override bool TryGetValue(IBindingRequest source, [NotNullWhen(true)] out object? value)
    {
        if (RequestingAtTypeThisImplements(source.DesiredType))
        {
            value = this;
            return true;
        }
        return base.TryGetValue(source, out value);
    }

    private static bool RequestingAtTypeThisImplements(Type type)
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

    public void RegisterForDispose(object obj)
    {
        if (!RequestingAtTypeThisImplements(obj.GetType()))
            disposeContainer.RegisterForDispose(obj);
    }

    public bool SatisfiesDisposeRequirement => true;

    public void Dispose() => disposeContainer.Dispose();

    public bool AllowSingletonInside(Type request) =>
        RequestingAtTypeThisImplements(request);

    #endregion

    #region Asp.Net overrides

    public object? GetService(Type serviceType) => this.Get(serviceType);

    public object GetRequiredService(Type serviceType) =>
        GetService(serviceType) ?? 
        throw new InvalidOperationException(
            $"Service of type {serviceType} could not be constructed");

    public IServiceProvider ServiceProvider => this;
    public IServiceScope CreateScope() => new ServiceProviderSharingScope(ParentScope);

    public bool IsService(Type serviceType) => this.CanGet(serviceType);

    #endregion
}