using System;
using System.Collections.Generic;
using System.Reflection;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

namespace Melville.IOC.TypeResolutionPolicy;

public interface IRegisterGeneric
{
    void Register(Type source, Type destination, 
        Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector, 
        Action<ITypesafeActivationOptions<object>>? options = null);
    void RegisterIfNeeded(Type source, Type destination, 
        Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector, 
        Action<ITypesafeActivationOptions<object>>? options = null);
}

public class GenericResolutionPolicy: ITypeResolutionPolicy, IRegisterGeneric
{
    private readonly CachedResolutionPolicy cache; 
    private readonly Dictionary<Type, GenericActivation> registrations = new Dictionary<Type, GenericActivation>();

    public GenericResolutionPolicy(CachedResolutionPolicy cache)
    {
        this.cache = cache;
    }

    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
    {
        if (!request.DesiredType.IsGenericType) return null;
        var generic = request.DesiredType.GetGenericTypeDefinition();
        if (registrations.TryGetValue(generic, out var activation))
        {
            return activation.GetStrategy(request, cache);
        }
        return null;
    }

    public void RegisterIfNeeded(Type source, Type destination,
        Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector, 
        Action<ITypesafeActivationOptions<object>>? options = null)
    {
        if (!registrations.ContainsKey(source))
        {
            Register(source, destination, constructorSelector, options);
        }
    }
    public void Register(Type source, Type destination, 
        Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector, 
        Action<ITypesafeActivationOptions<object>>? options = null)
    {
        if (!source.IsGenericTypeDefinition) throw new InvalidOperationException($"{source.Name} is not a generic type");
        if (!destination.IsGenericTypeDefinition) throw new InvalidOperationException($"{destination.Name} is not a generic type");
        if (source.GetGenericArguments().Length != destination.GetGenericArguments().Length)
            throw new InvalidOperationException($"{source.Name} and {destination.Name} have different numbers of generic arguments");
            
        registrations[source] = new GenericActivation(destination, constructorSelector, options);               
    }
}

public class GenericActivation
{
    private readonly Type genericTemplate;
    private readonly Action<ITypesafeActivationOptions<object>>? options;
    private readonly Func<IList<ConstructorInfo>, IActivationStrategy> constructorSelector;

    public GenericActivation(Type genericTemplate, 
        Func<IList<ConstructorInfo>, IActivationStrategy> constructorSelector, 
        Action<ITypesafeActivationOptions<object>>? options)
    {
        this.genericTemplate = genericTemplate;
        this.options = options;
        this.constructorSelector = constructorSelector;
    }

    public IActivationStrategy? GetStrategy(IBindingRequest request, CachedResolutionPolicy cache)
    {
        var finalTargetType = genericTemplate.MakeGenericType(request.DesiredType.GetGenericArguments());
        var activator = cache.Bind(request.DesiredType, BindingPriority.KeepOld).DoBinding(
            TypeActivatorFactory.CreateTypeActivator(
                finalTargetType, constructorSelector)
        );
        options?.Invoke(activator);
        return activator.GetFinalFactory();
    }

}