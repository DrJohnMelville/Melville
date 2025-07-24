using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers;

public class ChangeDisposeRegistration : ForwardingRequest
{
    private readonly IRegisterDispose disposer;

    private ChangeDisposeRegistration(IBindingRequest inner, IRegisterDispose disposer) : base(inner)
    {
        this.disposer = disposer;
    }

    public override IRegisterDispose DisposeScope => disposer;

    public static IRegisterDispose TryDisposeChange(IRegisterDispose old, IRegisterDispose newDisposer) =>
        old.IsDisposalContainer ? old : newDisposer;

    public static IBindingRequest TryDisposeChange(IBindingRequest inner, IRegisterDispose disposer) =>
        inner.DisposeScope.IsDisposalContainer ? inner : ForceDisposeChange(inner, disposer);
    
    public static IBindingRequest ForceDisposeChange(IBindingRequest inner, IRegisterDispose disposer) =>
        inner.DisposeScope == disposer ? inner : new ChangeDisposeRegistration(inner, disposer);
}