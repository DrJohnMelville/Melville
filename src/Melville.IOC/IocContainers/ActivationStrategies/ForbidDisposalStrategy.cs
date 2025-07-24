using System;
using System.Linq;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ChildContainers;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public sealed class ForbidDisposalStrategy(
       IActivationStrategy inner, bool forbidDisposeEvenIfInScope)
    : ForwardingActivationStrategy(inner)
{
    public override object? Create(IBindingRequest bindingRequest) => 
        InnerActivationStrategy.Create(WrapIfNeeded(bindingRequest));

    private IBindingRequest WrapIfNeeded(IBindingRequest req) =>
        forbidDisposeEvenIfInScope ?
            ChangeDisposeRegistration.ForceDisposeChange(req, PreventDisposal.Instance):
            ChangeDisposeRegistration.TryDisposeChange(req, PreventDisposal.Instance);
}