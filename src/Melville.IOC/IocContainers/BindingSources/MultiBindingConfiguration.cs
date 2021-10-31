using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers.BindingSources;

public class MultiBindingConfiguration<TSource>: IPickBindingTarget<TSource>
{
    private readonly List<Type> sources = new List<Type>();
    private readonly BindingRegistry registry;
    private readonly bool ifNeeded;

    public MultiBindingConfiguration(BindingRegistry registry, bool ifNeeded)
    {
        this.registry = registry;
        this.ifNeeded = ifNeeded;
        sources.Add(typeof(TSource));
    }
    public override IActivationOptions<TSource> DoBinding(IActivationStrategy strategy)
    {
        return registry.Bind<TSource>(sources, strategy, ifNeeded);
    }
        
    public override IPickBindingTarget<TSource> And<TDestination>()
    {
        sources.Add(typeof(TDestination));
        return this;
    }
}