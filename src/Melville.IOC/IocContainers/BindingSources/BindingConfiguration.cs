using System;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers.BindingSources;

public class BindingConfiguration<TSource>: IPickBindingTarget<TSource>
{
    private readonly Type targetType;
    protected BindingRegistry Registry { get; }
    protected BindingPriority Priority { get; }

    public BindingConfiguration(BindingRegistry registry, BindingPriority priority):
        this(typeof(TSource), registry, priority){}
        
    public BindingConfiguration(Type targetType, BindingRegistry registry, BindingPriority priority)
    {
        this.targetType = targetType;
        Registry = registry;
        Priority = priority;
    }

    public override IActivationOptions<TSource> DoBinding(IActivationStrategy strategy) => 
        Registry.Bind<TSource>(targetType, strategy, Priority);

    public override IPickBindingTarget<TSource> And<TDestination>() =>
        new MultiBindingConfiguration<TSource>(Registry, Priority).And<TDestination>();

}