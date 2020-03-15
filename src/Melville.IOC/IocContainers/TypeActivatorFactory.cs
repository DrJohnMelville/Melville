using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Melville.IOC.Activation;


namespace Melville.IOC.IocContainers
{
    public static class TypeActivatorFactory
    {
        public static IActivationStrategy CreateTypeActivator(Type type)
        {
            ActivatableTypesPolicy.ThrowIfNotActivatable(type);
            var list = SortedConstructorList(type);
            return list.Count switch
            {
                0 => throw new IocException("Could not find any constructors for " + type.Name),
                1 => StrategyFromConstructor(list[0]),
                _ => new MultiConstructorStrategy(list)
            };
        }

        public static TypeActivationStrategy StrategyFromConstructor(ConstructorInfo constructor)
        {
            return new TypeActivationStrategy(
                ActivationCompiler.Compile(
                    constructor.DeclaringType??throw new InvalidDataException("Constructor has no declaring type")
                    , constructor), constructor.GetParameters());
        }

        private static IList<ConstructorInfo> SortedConstructorList(Type type) =>
            type
                .GetConstructors()
                .OrderByDescending(i => i.GetParameters().Length)
                .ToList();
    }

    public class MultiConstructorStrategy : IActivationStrategy
    {
        private List<Lazy<IActivationStrategy>> constructors;
        public MultiConstructorStrategy(IList<ConstructorInfo> list)
        {
            constructors = list
                .Select(i => new Lazy<IActivationStrategy>(() => TypeActivatorFactory.StrategyFromConstructor(i)))
                .ToList();
        }

        public bool CanCreate(IBindingRequest bindingRequest) => 
            constructors.Any(i => i.Value.CanCreate(bindingRequest.Clone()));

        public object? Create(IBindingRequest bindingRequest)
        {
            foreach (var constructor in constructors.Where(i=>i.Value.CanCreate(bindingRequest.Clone())))
            {
                try
                {
                    return constructor.Value.Create(bindingRequest.Clone());
                }
                catch (IocException)
                {
                    // try the next constructor until one works
                }
            }
            // if we get to here then we failed.  So we need to repeat the first try to let
            // the exception bubble up.
            return constructors[0].Value.Create(bindingRequest);
        }

        public Lifetime2 Lifetime() => constructors[0].Value.Lifetime();

        public bool ValidForRequest(IBindingRequest request) => constructors.Any(i => i.Value.ValidForRequest(request));
    }
}