using System;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public interface IPickBindingTargetSource
    {
        IPickBindingTarget<T> Bind<T>(bool ifNeeded);
        IPickBindingTarget Bind(Type type, bool ifNeeded);
    }

    public class CachedResolutionPolicy : ITypeResolutionPolicy, IPickBindingTargetSource
    {
        private readonly BindingRegistry registry = new BindingRegistry();

        public IPickBindingTarget<T> Bind<T>(bool ifNeeded) => new BindingConfiguration<T>(registry, ifNeeded);
        public IPickBindingTarget Bind(Type type, bool ifNeeded) => new BindingConfiguration(type, registry, ifNeeded);

        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => 
            registry.TryGetBinding(request.DesiredType, out var ret) && ret.ValidForRequest(request) ?
                ret : null;
    }
}