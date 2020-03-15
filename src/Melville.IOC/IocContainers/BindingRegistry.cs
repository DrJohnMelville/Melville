using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Melville.IOC.IocContainers
{
    public class BindingRegistry
    {
        private readonly Dictionary<Type, IActivationStrategy> bindings = new Dictionary<Type, IActivationStrategy>();

        private static ObjectFactory CreateObjectFactory(IActivationStrategy strategy) => 
            strategy is ObjectFactory factory?
                factory: 
                new ObjectFactory(strategy);

        public ObjectFactory Bind(IEnumerable<Type> types, IActivationStrategy strategy, bool ifNeeded)
        {
            var ret = CreateObjectFactory(strategy);
            foreach (var type in types)
            {
                RegisterActivationStrategy(type, ret, ifNeeded);
            }
            return ret;
        }

        public ObjectFactory Bind(Type type, IActivationStrategy strategy, bool ifNeeded)
        {
            var ret = CreateObjectFactory(strategy);
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