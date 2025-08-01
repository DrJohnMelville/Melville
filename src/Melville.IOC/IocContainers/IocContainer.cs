﻿using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.Debuggers;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers;

public interface IIocService
{ 
    bool CanGet(IBindingRequest request);
    object? Get(IBindingRequest request);

    IIocService? ParentScope { get; }
    IRegisterDispose DefaultDisposeRegistration { get; set; }
    bool AllowDisposablesInGlobalScope => 
        DefaultDisposeRegistration != RequireDisposal.Instance;
    IIocDebugger Debugger { get; }
    string Trace => this.IocStackTrace(null);
    IBindingRequest SwitchRequestToMyContext(IBindingRequest prior) =>
        new ChangeIocServiceRequest(prior, this);
}

public static class IocServiceOperations
{
    public static IEnumerable<IIocService> ScopeList(this IIocService? service)
    {
        while (service != null)
        {
            yield return service;
            service = service.ParentScope;
        }
    }
    // make it so we can create scopes on IocContainer without casting
    public static IDisposableIocService CreateScope(this IIocService service) =>
        new CombinedScope(service);

    public static IIocService CreateSharingScope(this IIocService service) => 
        new SharingScopeContainer(service);
    
    public static IDisposableIocService CreateLifetimeScope(this IIocService service) => 
        new DisposableIocService(service);

    public static bool CanGet<T>(this IIocService ioc, params object[] arguments) => 
        ioc.CanGet(typeof(T), arguments);
    public static bool CanGet(this IIocService ioc, Type type, params object[] arguments) => 
        ioc.CanGet(new RootBindingRequest(type, ioc, arguments));
        
    public static T Get<T>(this IIocService ioc, params object[] arguments) => 
        (T) (ioc.Get(typeof(T?), arguments) ??
             throw new IocException($"Could not ceate a {typeof(T)}"));
  
    public static object? Get(this IIocService ioc, Type serviceTppe, params object[] arguments) => 
        ioc.Get(new RootBindingRequest(serviceTppe, ioc, arguments));


    public static object?[] Get(this IIocService service, IList<IBindingRequest> requiredParameters)
    {
        object?[] argumentArray = new object[requiredParameters.Count()];
        service.Fill(argumentArray.AsSpan(), requiredParameters);
        return argumentArray;
    }
}

public class IocContainer: IBindableIocService, IIocService
{
    public ITypeResolutionPolicyList TypeResolver { get; }

    public IocContainer(ITypeResolutionPolicyList? typeResolver = null)
    {
        this.TypeResolver = typeResolver ?? new StandardTypeResolutionPolicy();
    }
        
    public IIocService? ParentScope => null;
    public virtual IRegisterDispose DefaultDisposeRegistration { get; set; } = RequireDisposal.Instance;
  
    public T ConfigurePolicy<T>() => TypeResolver.GetInstantiationPolicy<T>();
    public IInterceptionPolicy InterceptionPolicy => TypeResolver.InterceptionPolicy;
    public void AddTypeResolutionPolicyBefore<T>(ITypeResolutionPolicy policy) => 
        TypeResolver.AddResolutionPolicyBefore<T>(policy);
    public void AddTypeResolutionPolicyAfter<T>(ITypeResolutionPolicy policy) => 
        TypeResolver.AddResolutionPolicyAfter<T>(policy);
    public void RemoveTypeResolutionPolicy<T>() => TypeResolver.RemoveTypeResolutionPolicy<T>();

    public void AddTypeResolutionPolicyToEnd(ITypeResolutionPolicy policy) =>
        TypeResolver.AddTypeResolutionPolicyToEnd(policy);

    #region Get
        
    public object? Get(IBindingRequest bindingRequest)
    {
        Debugger.TypeRequested(bindingRequest);
        var activator = FindActivationStrategy(bindingRequest);
        var ret = activator.Create(bindingRequest);
        if (bindingRequest.IsCancelled)
        {
            if (ret is IDisposable disp) disp.Dispose();
            return null;
        }
        return ret;
    }

    private IActivationStrategy FindActivationStrategy(IBindingRequest bindingRequest) =>
        TypeResolver.ApplyResolutionPolicy(bindingRequest)??
        throw new IocException(bindingRequest.Print());

    #endregion

    #region CanGet

    public bool CanGet(IBindingRequest request)
    {
        try
        {
            return TypeResolver.ApplyResolutionPolicy(request)?.CanCreate(request) ?? false;
        }
        catch (Exception )
        {
            // if we run into a constructor we cannot automatically resolve, then we cannot get the type.
            return false;
        }
    }
    #endregion

    #region Debug Hook
    public IIocDebugger Debugger { get; set; } = SilentDebugger.Instance;
    #endregion
}