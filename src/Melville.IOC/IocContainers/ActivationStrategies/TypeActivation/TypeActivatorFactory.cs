using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.IOC.Activation;

namespace Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;

public static class TypeActivatorFactory
{
    public static IActivationStrategy CreateTypeActivator(Type type,
        Func<IList<ConstructorInfo>, ConstructorInfo> constructorSelector) =>
        CreateTypeActivator(type, i => constructorSelector(i).AsActivationStrategy());
        
    public static IActivationStrategy CreateTypeActivator(Type type,
        Func<IList<ConstructorInfo>,IActivationStrategy> constructorSelector)
    {
        ActivatableTypesPolicy.ThrowIfNotActivatable(type);
        var list = SortedConstructorList(type);
        if (list.Count < 1) throw new IocException("Could not find any constructors for " + type.Name);
        return constructorSelector(list);
    }

    public static TypeActivationStrategy StrategyFromConstructor(ConstructorInfo constructor)
    {
        return new TypeActivationStrategy(
            ConstructorInvoker.Create(constructor), constructor.GetParameters());
    }

    private static IList<ConstructorInfo> SortedConstructorList(Type type) =>
        type
            .GetConstructors()
            .OrderByDescending(i => i.GetParameters().Length)
            .ToList();
}
