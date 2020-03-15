using System;
using Melville.IOC.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Microsoft.Extensions.DependencyInjection;


namespace Melville.IOC.AspNet.RegisterFromServiceCollection
{
    public static class RegisterServiceCollectionWithContainer
    {
        public static void BindServiceCollection(this IocContainer container, IServiceCollection services)
        {
            foreach (var service in services)
            {
                BindSingleService(container, service);
            }
        }

        private static void BindSingleService(IocContainer container, ServiceDescriptor service)
        {
            if (service.ImplementationType?.IsGenericTypeDefinition ?? false)
            {
                BindOpenGeneric(container, service);
                return;
            }

            BindConcreteType(container, service);
        }

        private static void BindOpenGeneric(IocContainer container, ServiceDescriptor service)
        {
            container.ConfigurePolicy<IRegisterGeneric>().Register(service.ServiceType,
                service.ImplementationType, i => SetLifetime(i, service));
        }

        private static void BindConcreteType(IocContainer container, ServiceDescriptor service)
        {
            var bindingTarget = CreateBindingTarget(container, service);
            var activator = CreateActivator(bindingTarget, service);
            SetLifetime(activator, service);
        }

        private static void SetLifetime(IActivationOptions activator, ServiceDescriptor service)
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

        private static IPickBindingTarget CreateBindingTarget(IocContainer container, ServiceDescriptor service)
        {
            return container.ConfigurePolicy<IPickBindingTargetSource>()
                .Bind(service.ServiceType, false);
        }

        private static IActivationOptions CreateActivator(IPickBindingTarget target, ServiceDescriptor service) =>
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