using System.Diagnostics.CodeAnalysis;
using Melville.INPC;
using Melville.IOC.BindingRequests;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.IocContainers;

[StaticSingleton]
public partial class RootScope : IScope
{
    /// <inheritdoc />
    public bool TryGetValue(IBindingRequest source, IActivationStrategy key, [NotNullWhen(true)] out object? result)
    {
        result = null;
        return false;
    }

    /// <inheritdoc />
    public bool TrySetValue(IBindingRequest source, IActivationStrategy key, object? value) => false;
 }