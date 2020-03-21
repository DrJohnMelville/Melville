namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public class ForwardingActivationStrategy : IActivationStrategy
    {
        private IActivationStrategy inner;

        public ForwardingActivationStrategy(IActivationStrategy inner)
        {
            this.inner = inner;
        }

        public bool CanCreate(IBindingRequest bindingRequest) => 
            inner.CanCreate(bindingRequest);

        public virtual object? Create(IBindingRequest bindingRequest) => 
            inner.Create(bindingRequest);
        public virtual SharingScope SharingScope() => inner.SharingScope();
        public virtual bool ValidForRequest(IBindingRequest request) => inner.ValidForRequest(request);
    }
}