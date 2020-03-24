using System;
using System.Runtime.Serialization;
using Melville.IOC.IocContainers.BindingSources;
using Melville.IOC.TypeResolutionPolicy;

namespace Melville.IOC.IocContainers
{
    public interface IBindableIocService 
    {
        T ConfigurePolicy<T>();
    }

    public static class BindableIocServiceOperations
    {
        public static IPickBindingTarget<T> Bind<T>(this IBindableIocService service) => 
            ClosedTypeBindings<T>(service).Bind<T>(false);
        public static IPickBindingTarget<T> BindIfMNeeded<T>(this IBindableIocService service) => 
            ClosedTypeBindings<T>(service).Bind<T>(true);

        private static IPickBindingTargetSource ClosedTypeBindings<T>(IBindableIocService service) => 
            service.ConfigurePolicy<IPickBindingTargetSource>();

        public static void BindGeneric(this IBindableIocService services, Type source, Type destination,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            services.ConfigurePolicy<IRegisterGeneric>().Register(source, destination, options);
        public static void BindGenericIfNeeded(this IBindableIocService services, Type source, Type destination,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            services.ConfigurePolicy<IRegisterGeneric>().RegisterIfNeeded(source, destination, options);
    }
}