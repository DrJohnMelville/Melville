using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

public class ScopeRegistry: Dictionary<IActivationStrategy, object?>, IScope
{
    public bool TryGetValue(
        IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result) =>
        TryGetValue(key, out result);

    public bool TrySetValue(IBindingRequest source, IActivationStrategy key, object? value)
    {
        this[key] = value;
        return true;
    }
}