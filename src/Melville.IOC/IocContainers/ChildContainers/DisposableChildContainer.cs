using System;
using Melville.IOC.BindingRequests;
using System.Threading.Tasks;

namespace Melville.IOC.IocContainers.ChildContainers;

public class DisposableChildContainer(IBindableIocService parent, IIocService parentServ)
    : ChildContainer(parent, parentServ), 
        IDisposableIocService, IRegisterDispose
{
    private readonly DisposalRegister innerService = new();

    public ValueTask DisposeAsync() => innerService.DisposeAsync();
    public void Dispose() => innerService.Dispose();
    public void RegisterForDispose(object obj)
    {
        if (obj == this) return;
        innerService.RegisterForDispose(obj);
    }

    public bool SatisfiesDisposeRequirement => innerService.SatisfiesDisposeRequirement;

}