using Melville.IOC.BindingRequests;
using System.Diagnostics.CodeAnalysis;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public class AllowScopedInsideSingletonActivationStrategy(
    IActivationStrategy innerActivationStrategy) : ForwardingActivationStrategy(innerActivationStrategy)
{
    public override object? Create(IBindingRequest bindingRequest) => 
        base.Create(new AllowScopedInsideSingleton(bindingRequest));
}

// this lets a singleton try to adopt values from the scope in which it was originally created.  This
// is dangerous if the dependency includes IDisposables, because the singleton may have a dependency
// disposed of out from under it.  It is safe to use the dependency in the constructor.  It is also safe
// for some special scoped values declared in .NET ioc containers that last as long as the container.
// 
// The getter searches the scope list for any instances.  If none are found the writer will attempt
// to write to the scope.  If the return on the writer is false (either because we are inside of a
// singleton or there is no scope, this method forces the scope to true, and the object essentially
// becomes a transient object with respect to sharing scopes.
//
// The TryGetValue relies on behavior that CreateSingletonRequest will probe for scopes
// beyond the singleton, and return a nonnull result with a false return value.  TryGetValue
// detects this condition and changes the function result to true, which rescues the failed request.
public class AllowScopedInsideSingleton(IBindingRequest parent) : ForwardingRequest(parent), IScope
{
    private IScope PriorSharingScope => base.SharingScope;
    public override IScope SharingScope => this;

    public bool TryGetValue(
        IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result) =>
        PriorSharingScope.TryGetValue(source, key, out result) || 
        result is not null;

    /// <inheritdoc />
    public bool TrySetValue(IBindingRequest source, IActivationStrategy key, object? value) => 
        PriorSharingScope.TrySetValue(source, key, value) || true;
}