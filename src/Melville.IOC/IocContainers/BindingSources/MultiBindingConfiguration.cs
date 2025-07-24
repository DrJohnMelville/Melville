using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers.BindingSources;

public class MultiBindingConfiguration<TSource>: IPickBindingTarget<TSource>
{
    private readonly List<Type> sources = new List<Type>();
    private readonly BindingRegistry registry;
    private readonly BindingPriority priority;

    public MultiBindingConfiguration(BindingRegistry registry, BindingPriority priority)
    {
        this.registry = registry;
        this.priority = priority;
        sources.Add(typeof(TSource));
    }
    public override IActivationOptions<TSource> DoBinding(IActivationStrategy strategy)
    {
        return registry.Bind<TSource>(sources, strategy, priority);
    }
        
    public override IPickBindingTarget<TSource> And<TDestination>()
    {
        sources.Add(typeof(TDestination));
        return this;
    }
}