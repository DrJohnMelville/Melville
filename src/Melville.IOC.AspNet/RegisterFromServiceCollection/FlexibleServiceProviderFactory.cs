using System;
using System.Collections;
using System.Collections.Generic;
using Melville.INPC;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.Debuggers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.IOC.AspNet.RegisterFromServiceCollection;

public class FlexibleServiceProviderFactory
    (bool allowRootDisposables, IServiceCollection services) :
    IServiceProviderFactory<IocContainer>,
    IBindableIocService, IServiceCollection
{
    private IServiceCollection? anticipatedServices = services;
    private IServiceCollection Services =>
    anticipatedServices ?? throw new InvalidOperationException(
        "Cannot register with servicecollection extensions after building the app.");
    private readonly IocContainer container = new();

    #region ServiceProviderFactory Implementation

    IocContainer IServiceProviderFactory<IocContainer>.CreateBuilder(
        IServiceCollection services)
    {
        if (services != anticipatedServices)
            throw new InvalidOperationException(
                "Attempt to create with a different service collection than given in constructor.");
        container.BindServiceCollection(anticipatedServices);
        anticipatedServices = null; 
        return container;
    }

    IServiceProvider IServiceProviderFactory<IocContainer>.CreateServiceProvider(
        IocContainer containerBuilder) =>
        containerBuilder.CreateServiceProvider(allowRootDisposables);

    #endregion

    #region ServiceCollectionImplementation

    public IEnumerator<ServiceDescriptor> GetEnumerator() => Services.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Services).GetEnumerator();

    public void Add(ServiceDescriptor item) => Services.Add(item);

    public void Clear() => Services.Clear();

    public bool Contains(ServiceDescriptor item) => Services.Contains(item);

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => Services.CopyTo(array, arrayIndex);

    public bool Remove(ServiceDescriptor item) => Services.Remove(item);

    public int Count => Services.Count;

    public bool IsReadOnly => Services.IsReadOnly;

    public int IndexOf(ServiceDescriptor item) => Services.IndexOf(item);

    public void Insert(int index, ServiceDescriptor item) => Services.Insert(index, item);

    public void RemoveAt(int index) => Services.RemoveAt(index);

    public ServiceDescriptor this[int index]
    {
        get => Services[index];
        set => Services[index] = value;
    }

    #endregion

    #region IBindableIocService Implementation

    public T ConfigurePolicy<T>() => container.ConfigurePolicy<T>();

    public IInterceptionPolicy InterceptionPolicy => container.InterceptionPolicy;

    public void AddTypeResolutionPolicyBefore<T>(ITypeResolutionPolicy policy) => container.AddTypeResolutionPolicyBefore<T>(policy);

    public void AddTypeResolutionPolicyAfter<T>(ITypeResolutionPolicy policy) => container.AddTypeResolutionPolicyAfter<T>(policy);

    public void RemoveTypeResolutionPolicy<T>() => container.RemoveTypeResolutionPolicy<T>();
    #endregion

    public void SetDebugHook(IIocDebugger debugger) =>
        container.Debugger = debugger;

    public IocContainer IocContainer => container;
}