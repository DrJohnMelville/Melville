using Melville.IOC.IocContainers;

namespace Melville.IOC.TypeResolutionPolicy
{
    public class DefaultValuePolicy : ITypeResolutionPolicy
    {
        public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => request.HasDefaultValue(out var value) ? new ConstantActivationStrategy(value) : null;
    }
}