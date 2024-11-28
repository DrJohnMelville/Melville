using System;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.TypeResolutionPolicy;

public class FailRequestPolicy : ITypeResolutionPolicy, IActivationStrategy
{
    public IActivationStrategy? ApplyResolutionPolicy(IBindingRequest request) => this;

    public bool CanCreate(IBindingRequest bindingRequest) => false;

    public object? Create(IBindingRequest bindingRequest)
    {
        bindingRequest.IsCancelled = true;
        bindingRequest.IocService.Debugger.ResolutionFailed(bindingRequest);
        return null;
    }

    public SharingScope SharingScope() => IocContainers.SharingScope.Transient;

    public bool ValidForRequest(IBindingRequest request) => true;
}