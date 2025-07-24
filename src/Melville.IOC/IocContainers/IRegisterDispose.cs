namespace Melville.IOC.IocContainers;

public interface IRegisterDispose
{
    void RegisterForDispose(object obj);
    bool IsDisposalContainer { get; }
}