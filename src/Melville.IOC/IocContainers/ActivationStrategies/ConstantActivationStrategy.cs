namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public sealed class ConstantActivationStrategy: IActivationStrategy
    {
        private readonly object? value;

        public ConstantActivationStrategy(object? value)
        {
            this.value = value;
        }

        public SharingScope SharingScope() => IocContainers.SharingScope.Singleton;
        public bool ValidForRequest(IBindingRequest request) => true;
        public bool CanCreate(IBindingRequest bindingRequest) => true;
        public (object? Result, DisposalState DisposalState) Create(IBindingRequest bindingRequest) => 
            (value, DisposalState.DisposalDone);
    }
}