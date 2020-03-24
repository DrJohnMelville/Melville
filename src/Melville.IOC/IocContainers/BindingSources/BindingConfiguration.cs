using System;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers.BindingSources
{
    public class BindingConfiguration<TSource>: IPickBindingTarget<TSource>
    {
        private readonly Type targetType;
        protected BindingRegistry Registry { get; }
        protected bool IfNeeded { get; }

        public BindingConfiguration(BindingRegistry registry, bool ifNeeded):
            this(typeof(TSource), registry, ifNeeded){}
        
        public BindingConfiguration(Type targetType, BindingRegistry registry, bool ifNeeded)
        {
            this.targetType = targetType;
            Registry = registry;
            IfNeeded = ifNeeded;
        }

        public override IActivationOptions<TSource> DoBinding(IActivationStrategy strategy) => 
            Registry.Bind<TSource>(targetType, strategy, IfNeeded);

        public override IPickBindingTarget<TSource> And<TDestination>() =>
        new MultiBindingConfiguration<TSource>(Registry, IfNeeded).And<TDestination>();

    }
}