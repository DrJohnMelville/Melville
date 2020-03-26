using System.Collections.Generic;
using System.Linq;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.InjectionPolicies
{
    public interface IInterceptionPolicy
    {
        IInterceptionRule AsInterceptionRule();
        void Add(IInterceptionRule rule);
    }

    public interface IInterceptionRule
    {
        object? Intercept(IBindingRequest request, object? source);
    }
    public class DefaultInterceptionPolicy: IInterceptionPolicy, IInterceptionRule
    {
        private readonly List<IInterceptionRule> rules = new List<IInterceptionRule>();

        public DefaultInterceptionPolicy()
        {
            rules.Add(new AttemptDisposeRule());
        }

        public object? Intercept(IBindingRequest request, object? source) => 
            rules.Aggregate(source, (item, rule) => rule.Intercept(request, item));

        public IInterceptionRule AsInterceptionRule() => this;
        public void Add(IInterceptionRule rule) => rules.Add(rule);
    }
}