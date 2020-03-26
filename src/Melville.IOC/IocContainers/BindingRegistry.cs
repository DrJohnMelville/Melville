using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.InjectionPolicies;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers
{
    public class BindingRegistry
    {
        private readonly Dictionary<Type, IActivationStrategy> bindings = new Dictionary<Type, IActivationStrategy>();
        private readonly IInjectionRule injectionPolicy;

        public BindingRegistry(IInjectionRule injectionPolicy)
        {
            this.injectionPolicy = injectionPolicy;
        }

        private ObjectFactory<T> CreateObjectFactory<T>(IActivationStrategy strategy)
        {
            if (strategy is ObjectFactory)
                throw new InvalidOperationException("tried to register a factory");
            
                return new ObjectFactory<T>(strategy, injectionPolicy);
        }

        public ObjectFactory<T> Bind<T>(IEnumerable<Type> types, IActivationStrategy strategy, bool ifNeeded)
        {
            var ret = CreateObjectFactory<T>(strategy);
            foreach (var type in types)
            {
                RegisterActivationStrategy(type, ret, ifNeeded);
            }
            return ret;
        }

        public ObjectFactory<T> Bind<T>(Type type, IActivationStrategy strategy, bool ifNeeded)
        {
            var ret = CreateObjectFactory<T>(strategy);
            RegisterActivationStrategy(type, ret, ifNeeded);
            return ret;
        }

        private void RegisterActivationStrategy(Type type, IActivationStrategy ret, bool ifNeeded)
        {
            var existing = bindings.TryGetValue(type, out var e) ? e : null;
            switch (existing, ifNeeded)
            {
                case (null, _): bindings[type] = ret;
                    break;
                case (_, true): break;
                case (MultipleActivationStrategy mu, _): mu.AddStrategy(ret);
                    break;
                default:
                    //The first clause above tests that existing is not null
                    var newMu = new MultipleActivationStrategy(existing!);
                    newMu.AddStrategy(ret);
                    bindings[type] = newMu;
                    break;
            }
        }

        public bool TryGetBinding(Type key, [NotNullWhen(true)] out IActivationStrategy? output) => bindings.TryGetValue(key, out output);
    }
}