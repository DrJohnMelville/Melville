using System.Linq;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public class ScopedActivationStrategy : ForwardingActivationStrategy
    {
        public ScopedActivationStrategy(IActivationStrategy innerActivationStrategy): base(innerActivationStrategy)
        {
            if (innerActivationStrategy.SharingScope() != IocContainers.SharingScope.Transient)
            {
                throw new IocException("Bindings may only specify at most one lifetime.");
            }
        }

        public override SharingScope SharingScope() => IocContainers.SharingScope.Scoped;
    
        private IScope Scope(IBindingRequest req) => 
            req.IocService.ScopeList().OfType<IScope>().FirstOrDefault()??
            throw new IocException($"Attempted to create a scoped {req.DesiredType.Name} outside of a scope.");

        public override (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest)=>
            Scope(bindingRequest).TryGetValue(this, out var ret) ? 
                (ret, DisposalState.DisposalDone) : // presumably the value got registered for disposal when created 
                RecordScopedValue(bindingRequest, base.Create(bindingRequest));

        private (object? Result, DisposalState DisposalState) 
            RecordScopedValue(IBindingRequest bindingRequest, in (object? Result, DisposalState DisposalState) create)
        {
            Scope(bindingRequest).SetScopeValue(this, create.Result);
            return create;
        }
    }
}