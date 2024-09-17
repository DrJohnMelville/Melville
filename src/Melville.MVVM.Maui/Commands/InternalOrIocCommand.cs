using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Melville.INPC;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.Commands;

[FromConstructor]
internal partial class InternalOrIocCommand: InheritedCommand
{
    public override void Execute(object? parameter)
    {
        object? result = null;
        var contextFactory = GetIocContext(parameter);
        var context = contextFactory.CreateContext();
        try
        {
            result = InnerExecute(parameter, context);
        }
        finally
        {
            DoDispose(context, result);
        }        
    }

    private IIocContextFactory GetIocContext(object? parameter)
    {
        var current = parameter as Element;
        while (current is not null)
        {
            if (GetIocContextFactory(current) is { } context)
                return context;
            current = (current as Element)?.Parent;
        }
        throw new InvalidOperationException("Cannot find an IOC Context");
    }

    private static async void DoDispose(IIocContext context, object? result)
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