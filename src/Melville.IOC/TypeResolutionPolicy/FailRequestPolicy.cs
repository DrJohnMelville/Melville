using System;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy;

public class FailRequestPolicy : ITypeResolutionPolicy, IActivationStrategy
{
    public static event EventHandler<FailedRequestEventArgs>? BindingFailed;
    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request)
    {
        return this;
    }

    public bool CanCreate(IBindingRequest bindingRequest) => false;

    public object? Create(IBindingRequest bindingRequest)
    {
        bindingRequest.IsCancelled = true;
        BindingFailed?.Invoke(this, new FailedRequestEventArgs(bindingRequest));
        return null;
    }

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;

    public bool ValidForRequest(IBindingRequest request) => true;
}

public partial class FailedRequestEventArgs : EventArgs
{
    [FromConstructor] public IBindingRequest Request { get; }
}