namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public class ForwardingActivationStrategy : IActivationStrategy
    {
        protected IActivationStrategy InnerActivationStrategy { get; set; }

        public ForwardingActivationStrategy(IActivationStrategy innerActivationStrategy)
        {
            this.InnerActivationStrategy = innerActivationStrategy;
        }

        public bool CanCreate(IBindingRequest bindingRequest) => 
            InnerActivationStrategy.CanCreate(bindingRequest);

        public virtual (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest) => 
            InnerActivationStrategy.Create(bindingRequest);
        public virtual SharingScope SharingScope() => InnerActivationStrategy.SharingScope();
        public virtual bool ValidForRequest(IBindingRequest request) => InnerActivationStrategy.ValidForRequest(request);
    }
}