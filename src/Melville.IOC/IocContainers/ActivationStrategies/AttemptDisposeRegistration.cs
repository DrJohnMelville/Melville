using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.IOC.BindingRequests;

namespace Melville.IOC.IocContainers.ActivationStrategies;

public static class AttemptDisposeRegistrationExtensions
{
    public static object? TryRegisterDisposeAndReturn(this object? item, IBindingRequest br)
    {
        TryRegisterForDispose(br.IocService, item);
        return item;
    }
    public static void TryRegisterForDispose(this IIocService service, object? itme)
    {
        if (!IsDisposableItem(itme)) return;
        if (service.ScopeList().OfType<IRegisterDispose>().FirstOrDefault() is {} reg)
        {
            reg.RegisterForDispose(itme);
        }
        else if (!service.AllowDisposablesInGlobalScope)
        {
            throw new IocException($"Type {itme.GetType().Name} requires disposal but was created at global scope.");
        }
    }

    private static bool IsDisposableItem([NotNullWhen(true)] object? ret) =>
        ret is IDisposable || ret is IAsyncDisposable;

}