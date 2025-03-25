using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

public enum BindingPriority
{
    KeepBoth = 0,
    KeepOld = 1, // previously IfNeeded
    KeepNew = 2, 
}

public class BindingRegistry
{
    private readonly ConcurrentDictionary<Type, IActivationStrategy> bindings = new();
    private readonly IInterceptionRule interceptionPolicy;

    public BindingRegistry(IInterceptionRule interceptionPolicy)
    {
        this.interceptionPolicy = interceptionPolicy;
    }

    private ObjectFactory<T> CreateObjectFactory<T>(IActivationStrategy strategy)
    {
        if (strategy is ObjectFactory)
            throw new InvalidOperationException("tried to register a factory");
            
        return new ObjectFactory<T>(strategy, interceptionPolicy);
    }

    public ObjectFactory<T> Bind<T>(
        IEnumerable<Type> types, IActivationStrategy strategy, BindingPriority priority)
    {
        var ret = CreateObjectFactory<T>(strategy);
        foreach (var type in types)
        {
            RegisterActivationStrategy(type, ret, priority);
        }
        return ret;
    }

    public ObjectFactory<T> Bind<T>(Type type, IActivationStrategy strategy, BindingPriority priority)
    {
        var ret = CreateObjectFactory<T>(strategy);
        RegisterActivationStrategy(type, ret, priority);
        return ret;
    }

    private void RegisterActivationStrategy(Type type, IActivationStrategy ret, BindingPriority priority)
    {
        var existing = bindings.TryGetValue(type, out var e) ? e : null;
        if ((existing?.FindSubstrategy<FinalActivationStrategy>() ?? []).Any())
        {
            if (priority is BindingPriority.KeepNew)
                throw new IocException("Cannot overwrite a final binding.");
            return;
        }
        switch (existing, priority)
        {
            case (null, _): bindings[type] = ret; // If nothing registered, register the first one
                break;
            case (_, BindingPriority.KeepOld): break; // if there is a prior  registration, and this is if needed quit
            case (_, BindingPriority.KeepNew):
                bindings[type] = ret; 
                break;
            case (MultipleActivationStrategy mu, _): mu.AddStrategy(ret); // if there is already a multistrategy, add to it,
                break;
            default: // otherwise, both existing and new strategies exist -- create a multistrategy to contain them both.
                //The first clause above tests that existing is not null
                bindings[type] = new MultipleActivationStrategy(existing, ret);
                break;
        }
    }

    public bool TryGetBinding(Type key, [NotNullWhen(true)] out IActivationStrategy? output) => bindings.TryGetValue(key, out output);
}