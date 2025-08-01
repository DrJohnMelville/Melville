﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;


namespace Melville.IOC.IocContainers;

public class MultipleActivationStrategy : IActivationStrategy
{
    private readonly List<IActivationStrategy> strateies = new List<IActivationStrategy>();

    public MultipleActivationStrategy(
        IActivationStrategy strategy1, IActivationStrategy strategy2)
    {
        strateies.Add(strategy1);
        strateies.Add(strategy2);
    }

    private IActivationStrategy? SelectActivator(IBindingRequest bindingRequest) => 
        strateies.LastOrDefault(i => i.ValidForRequest(bindingRequest));
        
    public void AddStrategy(IActivationStrategy strategy) => strateies.Add(strategy);

    public bool CanCreate(IBindingRequest bindingRequest) =>
        SelectActivator(bindingRequest)?.CanCreate(bindingRequest) ?? false;
         
    public object? Create(IBindingRequest bindingRequest)=>
        (SelectActivator(bindingRequest)??
         throw new IocException($"No binding for {bindingRequest.DesiredType.Name} is valid in this context.")
        ).Create(bindingRequest);

    public void CreateMany(IBindingRequest bindingRequest, IList accumulator)
    {
        foreach (var strategy in strateies)
        {
            if (!strategy.ValidForRequest(bindingRequest)) continue;
            var o = strategy.Create(bindingRequest);
            if (bindingRequest.IsCancelled)
            {
                if (o is IDisposable disp) disp.Dispose();
                continue;
            }
            if (o is null) continue;
            accumulator.Add(o);
        }
    }

    public SharingScope SharingScope() => strateies.Select(i => i.SharingScope()).Min();
    public bool ValidForRequest(IBindingRequest request) => SelectActivator(request) != null;

    /// <inheritdoc />
    public IEnumerable<T> FindSubstrategy<T> () where T : class
    {
        var ret = strateies.SelectMany(i => i.FindSubstrategy<T>());
        return this is T casted ? ret.Prepend(casted) : ret;
    }
}