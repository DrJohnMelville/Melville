﻿using System;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy;

public interface ITypeResolutionPolicy
{
    IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request);
}

public interface ITypeResolutionPolicyList:ITypeResolutionPolicy
{
    T GetInstantiationPolicy<T>();
    IInterceptionPolicy InterceptionPolicy { get; }
    void AddResolutionPolicyBefore<T>(ITypeResolutionPolicy policy);
    void AddResolutionPolicyAfter<T>(ITypeResolutionPolicy policy);
    void RemoveTypeResolutionPolicy<T>();
    void AddTypeResolutionPolicyToEnd(ITypeResolutionPolicy policy);
}
public class TypeResolutionPolicyList: ITypeResolutionPolicyList
{
    public List<ITypeResolutionPolicy> Policies { get; } = new List<ITypeResolutionPolicy>();
    public IInterceptionPolicy InterceptionPolicy { get; } = new DefaultInterceptionPolicy();
    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) =>
        Policies
            .Select(i => i.ApplyResolutionPolicy(request))
            .FirstOrDefault(i => i != null && i.ValidForRequest(request));


    public T GetInstantiationPolicy<T>() => Policies
                                                .Select(i=>i is MemorizeResult mr? mr.InnerPolicy:i)
                                                .OfType<T>()
                                                .FirstOrDefault()??
                                            throw new InvalidOperationException("No policy object of type: " + typeof(T).Name);

    public void AddResolutionPolicyBefore<T>(ITypeResolutionPolicy policy)
    {
        var index = Policies.FindIndex(IsResolver<T>);
        CheckIfItemFound(index);
        Policies.Insert(index, policy);
    }
    public void AddResolutionPolicyAfter<T>(ITypeResolutionPolicy policy)
    {
        var index = Policies.FindLastIndex(IsResolver<T>);
        CheckIfItemFound(index);
        Policies.Insert(index+1, policy);
    }

    private void CheckIfItemFound(int index)
    {
        if (index < 0)
        {
            throw new IocException("Could not find the target resolution policy in AddResolutionPolicyBefore");
        }
    }

    public void AddTypeResolutionPolicyToEnd(ITypeResolutionPolicy policy) => 
        Policies.Add(policy);

    public void RemoveTypeResolutionPolicy<T>() => Policies.RemoveAll(IsResolver<T>);

    private bool IsResolver<T>(ITypeResolutionPolicy item) => item is T ||
                                                              (item is MemorizeResult mr && mr.InnerPolicy is T);
}