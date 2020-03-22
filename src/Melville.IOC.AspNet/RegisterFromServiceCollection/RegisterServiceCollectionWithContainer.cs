using System;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.Extensions.DependencyInjection;


namespace Melville.IOC.AspNet.RegisterFromServiceCollection
{
    public static class RegisterServiceCollectionWithContainer
    {
        public static void BindServiceCollection(this IBindableIocService container,
            Action<IServiceCollection> collectionConfig)
        {
            var sc = new ServiceCollection();
            collectionConfig(sc);
            container.BindServiceCollection(sc);
        }
        public static void BindServiceCollection(this IBindableIocService container, IServiceCollection services)
        {
            foreach (var service in services)
            {
                BindSingleService(container, service);
            }
        }

        private static void BindSingleService(IBindableIocService container, ServiceDescriptor service)
        {
            if (service.ImplementationType?.IsGenericTypeDefinition ?? false)
            {
                BindOpenGeneric(container, service);
                return;
            }

            BindConcreteType(container, service);
        }

        private static void BindOpenGeneric(IBindableIocService container, ServiceDescriptor service)
        {
            container.ConfigurePolicy<IRegisterGeneric>().Register(service.ServiceType,
                service.ImplementationType, i => SetLifetime(i, service));
        }

        private static void BindConcreteType(IBindableIocService container, ServiceDescriptor service)
        {
            var bindingTarget = CreateBindingTarget(container, service);
            var activator = CreateActivator(bindingTarget, service);
            SetLifetime(activator, service);
        }

        private static void SetLifetime(ITypesafeActivationOptions<object> activator, ServiceDescriptor service)
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    activator.AsSingleton();
                    break;
                case ServiceLifetime.Scoped:
                    activator.AsScoped();
                    break;
                case ServiceLifetime.Transient:
                    //Intentionally do nothing, services default to transient.
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid Service Lifetime");
            }
        }

        private static IPickBindingTarget<object> CreateBindingTarget(IBindableIocService container, ServiceDescriptor service)
        {
            return container.ConfigurePolicy<IPickBindingTargetSource>()
                .Bind(service.ServiceType, false);
        }

        private static ITypesafeActivationOptions<object> CreateActivator(IPickBindingTarget<object> target, ServiceDescriptor service) =>
            service switch
            {
                var x when x.ImplementationType != null => target.ToType(service.ImplementationType),
                var x when x.ImplementationInstance != null => 
                              target.DoBinding(new ConstantActivationStrategy(service.ImplementationInstance)),
                var x when x.ImplementationFactory != null => 
                              target.DoBinding(new MethodActivationStrategy<object>
                                  ((container, request)=>service.ImplementationFactory(
                                  new ServiceProviderAdapter(container)))),
                _=>throw new InvalidOperationException(
                    "ServiceDescriptor must define one of ImplementationType, ImplementationInstance, or ImplementationFactory.")
            };
    }
}