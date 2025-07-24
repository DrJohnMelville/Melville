using Melville.INPC;

namespace Melville.IOC.IocContainers;

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