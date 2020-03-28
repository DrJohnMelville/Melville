using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies
{
    public interface IForbidDisposal
    {
    }

    public sealed class ConstantActivationStrategy: IActivationStrategy, IForbidDisposal
    {
        private readonly object? value;
        public ConstantActivationStrategy(object? value)
        {
            this.value = value;
        }

        public SharingScope SharingScope() => IocContainers.SharingScope.Singleton;
        public bool ValidForRequest(IBindingRequest request) => true;
        public bool CanCreate(IBindingRequest bindingRequest) => true;
        public object? Create(IBindingRequest bindingRequest) => 
            value;
    }
}