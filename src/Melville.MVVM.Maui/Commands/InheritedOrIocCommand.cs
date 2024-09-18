using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Melville.INPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.Commands;

[FromConstructor]
internal partial class InheritedOrIocCommand: InheritedCommand
{
    public override void Execute(object? parameter)
    {
        object? result = null;
        var contextFactory = GetIocContext(parameter);
        var context = contextFactory.CreateScope();
        try
        {
            result = InnerExecute(parameter, context.ServiceProvider);
        }
        finally
        {
            DoDispose(context, result);
        }        
    }

    private IServiceScopeFactory GetIocContext(object? parameter)
    {
        if (parameter is Element element &&
            element.Handler?.MauiContext?.Services.GetService(typeof(IServiceScopeFactory)) is IServiceScopeFactory isf)
            return isf;
        throw new InvalidOperationException("Cannot find an IOC Context");
    }

    private static async void DoDispose(IDisposable context, object? result)
    {
        switch (result)
        {
            case Task t: await t;
                break;
            case ValueTask vt:
                await vt;
                break;
            case { } x when IsValueTaskOfT(x):
                await (dynamic)x;
                break;
        }
        context.Dispose();
    }

    private static bool IsValueTaskOfT(object x) =>
        x.GetType() is {IsGenericType:true } type &&
        typeof(ValueTask<>) == type.GetGenericTypeDefinition();
}