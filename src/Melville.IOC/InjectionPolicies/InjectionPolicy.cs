using System.Collections.Generic;
using System.Linq;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.InjectionPolicies
{
    public interface IInjectionPolicy
    {
    }

    public interface IInjectionRule
    {
        object? Inject(IBindingRequest request, object? source);
    }
    public class DefaultInjectionPolicy: IInjectionPolicy, IInjectionRule
    {
        public List<IInjectionRule> Rules = new List<IInjectionRule>();

        public DefaultInjectionPolicy()
        {
            Rules.Add(new AttemptDisposeRule());
        }

        public object? Inject(IBindingRequest request, object? source) => 
            Rules.Aggregate(source, (item, rule) => rule.Inject(request, item));
    }
}