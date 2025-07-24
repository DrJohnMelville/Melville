using Melville.INPC;

namespace Melville.IOC.IocContainers;

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