using Melville.IOC.BindingRequests;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;

namespace Melville.IOC.IocContainers.ChildContainers;

public class
    DisposableChildContainer(IBindableIocService parent, IIocService parentServ)
    : ChildContainer(parent, parentServ),
        IDisposableIocService
{
    private readonly DisposalRegister innerService = new();

    public override IRegisterDispose DefaultDisposeRegistration
    {
        get => innerService;
        set => throw new InvalidOperationException("Cannot change disposal registration in a disposable scope");
    }

    public override bool CanGet(IBindingRequest request) =>
        base.CanGet(ChangeDisposeRegistration.ForceDisposeChange(request, innerService));

    public override object? Get(IBindingRequest request) =>
        base.Get(ChangeDisposeRegistration.ForceDisposeChange(request, innerService));

    public IBindingRequest SwitchRequestToMyContext(IBindingRequest prior)
    {
        return new SwitchIocAndDisposeContext(prior, this);
    }

    public ValueTask DisposeAsync() => innerService.DisposeAsync();
    public void Dispose() => innerService.Dispose();


    public class SwitchIocAndDisposeContext
        (IBindingRequest prior, DisposableChildContainer newContainer) : ForwardingRequest(prior)
    {
        public override IIocService IocService => newContainer;
        public override IRegisterDispose DisposeScope => newContainer.innerService;
    }
}