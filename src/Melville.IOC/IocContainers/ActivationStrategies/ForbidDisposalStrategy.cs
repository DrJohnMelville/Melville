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

    private IBindingRequest WrapIfNeeded (IBindingRequest req)=>
       (HasNoDisposeScope(req) || forbidDisposeEvenIfInScope)
           ? new ChangeScopeRequest(req, new DisposableIocService(
               req.IocService))
           : req;

    private static bool HasNoDisposeScope(IBindingRequest bindingRequest) => 
        !(bindingRequest.IocService.ScopeList()
            .OfType<IRegisterDispose>().
            FirstOrDefault()?.SatisfiesDisposeRequirement ?? false);
}

public partial class DoNotDuspose : IIocService, IRegisterDispose
{
    [FromConstructor][DelegateTo] private readonly IIocService inner;

    public void RegisterForDispose(object obj)
    {
        // do nothing
    }

    // this object says it will dispose of the given object, but it does not.
    public bool SatisfiesDisposeRequirement => true;
}