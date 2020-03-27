using Melville.IOC.InjectionPolicies;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class StandardTypeResolutionPolicy : TypeResolutionPolicyList
    {
        public StandardTypeResolutionPolicy()
        {
            var cache = new CachedResolutionPolicy(InterceptionPolicy.AsInterceptionRule());
            Policies.Add(new ArgumentBindingPolicy());
            Policies.Add(cache);
            Policies.Add(new MemorizeResult(cache, new GetIocServiceTypes()));
            Policies.Add(new GenericResolutionPolicy(cache));
            Policies.Add(new MemorizeResult(cache, new EnumerateMultipleBindingsPolicy(this)));
            Policies.Add(new MemorizeResult(cache, new FunctionsIntoFactories()));
            Policies.Add(new MemorizeResult(cache, new TuplesToScopeResolutionPolicy()));
            Policies.Add(new MemorizeResult(cache, new SelfBindByDefault()));
            Policies.Add(new MemorizeResult(cache, new ConventionResolutionPolicy()));
            Policies.Add(new DefaultValuePolicy());
        }
    }
}

