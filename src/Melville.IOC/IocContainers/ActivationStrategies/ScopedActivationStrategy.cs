using System.Linq;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public class ScopedActivationStrategy : ForwardingActivationStrategy
    {
        public ScopedActivationStrategy(IActivationStrategy inner): base(inner)
        {
            if (inner.SharingScope() != IocContainers.SharingScope.Transient)
            {
                throw new IocException("Bindings may only specify at most one lifetime.");
            }
        }

        public override SharingScope SharingScope() => IocContainers.SharingScope.Scoped;
    
        private IScope Scope(IBindingRequest req) => 
            req.IocService.ScopeList().OfType<IScope>().FirstOrDefault()??
            throw new IocException($"Attempted to create a scoped {req.DesiredType.Name} outside of a scope.");

        public override object? Create(IBindingRequest bindingRequest)=>
            Scope(bindingRequest).TryGetValue(this, out var ret) ? ret : 
                RecordScopedValue(bindingRequest, base.Create(bindingRequest));

        private object? RecordScopedValue(IBindingRequest bindingRequest, object? create)
        {
            Scope(bindingRequest).SetScopeValue(this, create);
            return create;
        }
    }
}