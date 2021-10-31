﻿using System;
using Melville.IOC.BindingRequests;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.BindingSources;

namespace Melville.IOC.TypeResolutionPolicy;

public interface IPickBindingTargetSource
{
    IPickBindingTarget<T> Bind<T>(bool ifNeeded);
    IPickBindingTarget<object> Bind(Type type, bool ifNeeded);
}

public interface ISetBackupCache
{
    void SetBackupCache(ITypeResolutionPolicy policy);
}
    
public sealed class CachedResolutionPolicy : ITypeResolutionPolicy, IPickBindingTargetSource, ISetBackupCache
{
    private readonly BindingRegistry registry;

    private ITypeResolutionPolicy? backupCache = null;

    public void SetBackupCache(ITypeResolutionPolicy policy) => backupCache = policy;

    public CachedResolutionPolicy(IInterceptionRule interceptionPolicy)
    {
        registry = new BindingRegistry(interceptionPolicy);
    }

    public IPickBindingTarget<T> Bind<T>(bool ifNeeded) => new BindingConfiguration<T>(registry, ifNeeded);

    public IPickBindingTarget<object> Bind(Type type, bool ifNeeded) => new BindingConfiguration<object>(type, registry, ifNeeded);

    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => 
        registry.TryGetBinding(request.DesiredType, out var ret) && ret.ValidForRequest(request) ?
            ret : backupCache?.ApplyResolutionPolicy(request);
}