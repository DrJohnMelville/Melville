using System;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.BindingSources;

namespace Melville.IOC.TypeResolutionPolicy
{
    public interface IPickBindingTargetSource
    {
        IPickBindingTarget<T> Bind<T>(bool ifNeeded);
        IPickBindingTarget<object> Bind(Type type, bool ifNeeded);
    }

    public class CachedResolutionPolicy : ITypeResolutionPolicy, IPickBindingTargetSource
    {
        private readonly BindingRegistry registry;

        public CachedResolutionPolicy(IInterceptionRule interceptionPolicy)
        {
            registry = new BindingRegistry(interceptionPolicy);
        }

        public IPickBindingTarget<T> Bind<T>(bool ifNeeded) => new BindingConfiguration<T>(registry, ifNeeded);
        public IPickBindingTarget<object> Bind(Type type, bool ifNeeded) => new BindingConfiguration<object>(type, registry, ifNeeded);

        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => 
            registry.TryGetBinding(request.DesiredType, out var ret) && ret.ValidForRequest(request) ?
                ret : null;
    }
}