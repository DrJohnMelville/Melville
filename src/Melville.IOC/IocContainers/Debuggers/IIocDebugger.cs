using Melville.INPC;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.Debuggers;

public interface IIocDebugger
{
    void TypeRequested(IBindingRequest request) { }
    void ResolutionFailed(IBindingRequest request) { }
    void ActivationRequested(IBindingRequest request) { }
}

[StaticSingleton]
public partial class SilentDebugger : IIocDebugger
{
}