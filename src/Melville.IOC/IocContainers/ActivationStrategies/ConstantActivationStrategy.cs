using Melville.INPC;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public sealed partial class ConstantActivationStrategy: IActivationStrategy
{
    [FromConstructor] private readonly object? value;
 
    public SharingScope SharingScope() => IocContainers.SharingScope.Singleton;
    public bool ValidForRequest(IBindingRequest request) => true;
    public bool CanCreate(IBindingRequest bindingRequest) => true;
    public object? Create(IBindingRequest bindingRequest) => 
        value;
}