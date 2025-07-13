using Melville.INPC;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers;

public interface IRegisterDispose
{
    void RegisterForDispose(object obj);
    bool IsDisposalContainer { get; }
}

[StaticSingleton]
public partial class PreventDisposal: IRegisterDispose
{
    public void RegisterForDispose(object obj)
    {
        // do nothing
    }
    // this object says it will dispose of the given object, but it does not.
    public bool IsDisposalContainer => false;
}

[StaticSingleton]
public partial class RequireDisposal: IRegisterDispose
{
    public void RegisterForDispose(object obj)
    {
        throw new IocException(
            $"Disposable object of type {obj.GetType()} was created in a scope that does not support disposal." );
    }
    public bool IsDisposalContainer => false;
}

public class ChangeDisposeRegistration : ForwardingRequest
{
    private readonly IRegisterDispose disposer;

    private ChangeDisposeRegistration(IBindingRequest inner, IRegisterDispose disposer) : base(inner)
    {
        this.disposer = disposer;
    }

    public override IRegisterDispose DisposeScope => disposer;

    public static IBindingRequest TryDisposeChange(IBindingRequest inner, IRegisterDispose disposer) =>
        inner.DisposeScope.IsDisposalContainer ? inner : ForceDisposeChange(inner, disposer);
    
    public static IBindingRequest ForceDisposeChange(IBindingRequest inner, IRegisterDispose disposer) =>
        inner.DisposeScope == disposer ? inner : new ChangeDisposeRegistration(inner, disposer);
} 