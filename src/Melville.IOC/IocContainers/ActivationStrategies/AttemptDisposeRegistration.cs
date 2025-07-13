using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public static class AttemptDisposeRegistrationExtensions
{
    public static object? TryRegisterDisposeAndReturn(this object? item, IBindingRequest br)
    {
        if (IsDisposableItem(item)) br.DisposeScope.RegisterForDispose(item);
        return item;
    }

    private static bool IsDisposableItem([NotNullWhen(true)] object? ret) =>
        ret is IDisposable || ret is IAsyncDisposable;

}