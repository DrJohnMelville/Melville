using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

namespace Melville.IOC.TypeResolutionPolicy
{

    public interface IRegisterGeneric
    {
        void Register(Type source, Type destination, Action<ITypesafeActivationOptions<object>>? options = null);
        void RegisterIfNeeded(Type source, Type destination, Action<ITypesafeActivationOptions<object>>? options = null);
    }

    public class GenericResolutionPolicy: ITypeResolutionPolicy, IRegisterGeneric
    {
        private readonly Dictionary<Type, GenericActivation> registrations = new Dictionary<Type, GenericActivation>(); 
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
        {
            if (!request.DesiredType.IsGenericType) return null;
            var generic = request.DesiredType.GetGenericTypeDefinition();
            if (registrations.TryGetValue(generic, out var activation))
            {
                return activation.GetStrategy(request.DesiredType.GetGenericArguments());
            }
            return null;
        }

        public void RegisterIfNeeded(Type source, Type destination, Action<ITypesafeActivationOptions<object>>? options = null)
        {
            if (!registrations.ContainsKey(source))
            {
                Register(source, destination, options);
            }
        }

        public void Register(Type source, Type destination, Action<ITypesafeActivationOptions<object>>? options = null)
        {
            if (!source.IsGenericTypeDefinition) throw new InvalidOperationException($"{source.Name} is not a generic type");
            if (!destination.IsGenericTypeDefinition) throw new InvalidOperationException($"{destination.Name} is not a generic type");
            if (source.GetGenericArguments().Length != destination.GetGenericArguments().Length)
                throw new InvalidOperationException($"{source.Name} and {destination.Name} have different numbers of generic arguments");
            
            registrations[source] = new GenericActivation(destination, options);               
        }
    }

    public class GenericActivation
    {
        private readonly Type genericTemplate;
        private readonly Action<ITypesafeActivationOptions<object>>? options;

        public GenericActivation(Type genericTemplate, Action<ITypesafeActivationOptions<object>>? options)
        {
            this.genericTemplate = genericTemplate;
            this.options = options;
        }

        public IActivationStrategy? GetStrategy(Type[] getGenericArguments)
        {
            var activator = CreateConcreteObjectFactory(getGenericArguments);
            ApplyConfigurationIfPresent(activator);
            return activator;
        }

        private ObjectFactory<object> CreateConcreteObjectFactory(Type[] getGenericArguments) =>
            new ObjectFactory<object>(
                TypeActivatorFactory.CreateTypeActivator(
                    genericTemplate.MakeGenericType(getGenericArguments), ConstructorSelectors.MaximumArgumentCount));

        private void ApplyConfigurationIfPresent(ObjectFactory<object> activator) => options?.Invoke(activator);
    }
}