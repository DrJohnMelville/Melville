using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.BindingRequests;
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

    public class DefaultInterceptionPolicy : IInterceptionPolicy, IInterceptionRule
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

    public abstract class InterceptorRuleBase<TSource, TDestination> : IInterceptionRule where TSource : TDestination
    {
        protected abstract TDestination DoInterception(IBindingRequest request, TSource source);

        public object? Intercept(IBindingRequest request, object? source)
        {
            if (!(DestinationTypeFulfillsRequest(request) && source is TSource legalSource)) return source;
            return DoInterception(request, legalSource);
        }

        private static bool DestinationTypeFulfillsRequest(IBindingRequest request) =>
            request.DesiredType.IsAssignableFrom(typeof(TDestination));
    }

    public class InterceptFromFunc<TSource, TDest> : InterceptorRuleBase<TSource, TDest> where TSource : TDest
    {
        private Func<TSource, TDest> transformation;

        public InterceptFromFunc(Func<TSource, TDest> transformation)
        {
            this.transformation = transformation;
        }

        protected override TDest DoInterception(IBindingRequest request, TSource source) 
            => transformation(source);
    }
}