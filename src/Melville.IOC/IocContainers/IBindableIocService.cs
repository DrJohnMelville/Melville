using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
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
            BindGeneric(services, source, destination, ConstructorSelectors.MaximumArgumentCount, options);
        public static void BindGeneric(this IBindableIocService services, Type source, Type destination,
            Func<IList<ConstructorInfo>, ConstructorInfo> constructorSelector,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            BindGeneric(services, source, destination, i => constructorSelector(i).AsActivationStrategy(), options);
        public static void BindGeneric(this IBindableIocService services, Type source, Type destination,
            Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            services.ConfigurePolicy<IRegisterGeneric>().Register(source, destination, constructorSelector, options);
        
        public static void BindGenericIfNeeded(this IBindableIocService services, Type source, Type destination,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            BindGenericIfNeeded(services, source, destination, ConstructorSelectors.MaximumArgumentCount, options);
        public static void BindGenericIfNeeded(this IBindableIocService services, Type source, Type destination,
            Func<IList<ConstructorInfo>, ConstructorInfo> constructorSelector,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            BindGenericIfNeeded(services, source, destination, i => constructorSelector(i).AsActivationStrategy(), options);
        public static void BindGenericIfNeeded(this IBindableIocService services, Type source, Type destination,
            Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector,
            Action<ITypesafeActivationOptions<object>>? options = null) =>
            services.ConfigurePolicy<IRegisterGeneric>().RegisterIfNeeded(source, destination, constructorSelector, options);
    }
}