using Melville.IOC.BindingRequests;


namespace Melville.IOC.IocContainers;

public class SharingScopeContainer(IIocService parentScope) : 
    GenericScope(parentScope)
{
    private readonly ScopeRegistry scopeItems = new();

    /// <inheritdoc />
    protected override IBindingRequest WrapRequest(IBindingRequest request) =>
        new ScopeChain(request, ParentScope, scopeItems, request.DisposeScope);
}