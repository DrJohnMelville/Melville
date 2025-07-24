using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.Debuggers;

namespace Melville.IOC.IocContainers;

public abstract class GenericScope(IIocService parentScope) : IIocService
{
    public IIocService ParentScope { get; } = parentScope;


    public IRegisterDispose DefaultDisposeRegistration
    {
        get => ParentScope.DefaultDisposeRegistration;
        set => ParentScope.DefaultDisposeRegistration = value;
    }

    public virtual bool CanGet(IBindingRequest request) => 
        ParentScope.CanGet(WrapRequest(request));

    public virtual object? Get(IBindingRequest request) => 
        ParentScope.Get(WrapRequest(request));

    public IIocDebugger Debugger => ParentScope.Debugger;

    protected abstract IBindingRequest WrapRequest(IBindingRequest request);
}