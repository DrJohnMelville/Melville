using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public sealed class ProhibitedActivationStrategy : IActivationStrategy
{
    public SharingScope SharingScope() => IocContainers.SharingScope.Singleton;
    public bool ValidForRequest(IBindingRequest request) => true;
    public bool CanCreate(IBindingRequest bindingRequest) => false;
    public object? Create(IBindingRequest bindingRequest) => null;
}