using System;
using System.Threading.Tasks;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers;

public interface IDisposableIocService : IIocService, IAsyncDisposable, IDisposable
{ 
}

public class DisposableIocService(IIocService parentScope) : GenericScope(parentScope), IDisposableIocService
{
    private readonly DisposalRegister register = new();

    /// <inheritdoc />
    protected override IBindingRequest WrapRequest(IBindingRequest request) => 
        new ScopeChain(request, ParentScope, RootScope.Instance, register);

    public ValueTask DisposeAsync() => register.DisposeAsync();
    public void Dispose() => register.Dispose();
}