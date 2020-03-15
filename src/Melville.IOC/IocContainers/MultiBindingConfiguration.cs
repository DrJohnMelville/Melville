using System;
using System.Collections.Generic;

namespace Melville.IOC.IocContainers
{
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
        public IActivationOptions DoBinding(IActivationStrategy strategy)
        {
            return registry.Bind(sources, strategy, ifNeeded);
        }
        
        public IActivationOptions To<TDestination>() where TDestination : TSource
        {
            return DoBinding(TypeActivatorFactory.CreateTypeActivator(typeof(TDestination)));
        }


        public IPickBindingTarget<TSource> And<TDestination>()
        {
            sources.Add(typeof(TDestination));
            return this;
        }
    }
}