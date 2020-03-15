namespace Melville.IOC.TypeResolutionPolicy
{
    public class StandardTypeResolutionPolicy : TypeResolutionPolicyList
    {
        public StandardTypeResolutionPolicy()
        {
            var cache = new CachedResolutionPolicy();
            Policies.Add(new LiteralBindingPolicy());
            Policies.Add(cache);
            Policies.Add(new MemorizeResult(cache, new GetIocServiceTypes()));
            Policies.Add(new MemorizeResult(cache, new GenericResolutionPolicy()));
            Policies.Add(new MemorizeResult(cache, new EnumerateMultipleBindingsPolicy(this)));
            Policies.Add(new MemorizeResult(cache, new FunctionsIntoFactories()));
            Policies.Add(new MemorizeResult(cache, new TuplesToScopeResolutionPolicy()));
            Policies.Add(new MemorizeResult(cache, new SelfBindByDefault()));
            Policies.Add(new MemorizeResult(cache, new ConventionResolutionPolicy()));
            Policies.Add(new DefaultValuePolicy());
        }
    }
}

