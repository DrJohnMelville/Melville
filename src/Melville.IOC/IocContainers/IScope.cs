using System.Diagnostics.CodeAnalysis;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

public interface IScope
{
    bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result);
    bool TrySetValue(IBindingRequest source, object? value, IActivationStrategy key);
}