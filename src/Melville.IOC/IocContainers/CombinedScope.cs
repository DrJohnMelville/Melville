using System.Threading.Tasks;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers;

public class CombinedScope(IIocService parent) : GenericScope(parent), IDisposableIocService
{
    protected readonly ScopeRegistry scopeItems = new();
    protected readonly DisposalRegister register = new();

    /// <inheritdoc />
    protected override IBindingRequest WrapRequest(IBindingRequest request) => 
        new ScopeChain(request, ParentScope, WrapScope(scopeItems),
            ChangeDisposeRegistration.TryDisposeChange(request.DisposeScope, register));

    protected virtual IScope WrapScope(IScope inner) => inner;

    /// <inheritdoc />
    public void Dispose() => register.Dispose();

    /// <inheritdoc />
    public ValueTask DisposeAsync() => register.DisposeAsync();
}

public class MandatoryDisposeScope(IIocService parent) : CombinedScope(parent)
{
    protected override IBindingRequest WrapRequest(IBindingRequest request) =>
        new ScopeChain(request, ParentScope, WrapScope(scopeItems), register);
}