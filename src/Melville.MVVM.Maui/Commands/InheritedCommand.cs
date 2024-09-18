using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.INPC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace Melville.MVVM.Maui.Commands;

[GenerateBP(typeof(object), "InheritedCommandParameter",
    Attached = true, Default = null, Nullable = true,
    XmlDocumentation = "Add an additional candidate to the inherited command chain")]
public partial class InheritedCommand :CommandBase
{
    private readonly Delegate action;
    private readonly ParameterInfo[] parameters;

    internal InheritedCommand(Delegate action)
    {
        this.action = action;
        parameters = action.Method.GetParameters();
    }

    public override void Execute(object? parameter)
    {
        InnerExecute(parameter, InvalidIocContext.Instance);
    }

    protected object? InnerExecute(object? parameter, IServiceProvider context)
    {
        var arguments = parameters
            .Select(i => 
                GetFromIoc(i)?
                    context.GetService(i.ParameterType) :
                GetValueFor(i.ParameterType, parameter))
            .ToArray();
        return action.GetMethodInfo().Invoke(action.Target, arguments);
    }

    private static bool GetFromIoc(ParameterInfo i)
    {
        return i.GetCustomAttribute<FromServicesAttribute>() is not null;
    }

    private object GetValueFor(Type desiredType, object? parameter)
    {
        return SearchValues(parameter)
            .First(desiredType.IsInstanceOfType);
    }

    private IEnumerable<object> SearchValues(object? root)
    {
        var current = root;
        while (current != null)
        {
            yield return current;
            if (current is BindableObject bo)
            {
                yield return bo.BindingContext;
                if (GetInheritedCommandParameter(bo) is { } explicitParam)
                {
                    yield return explicitParam;

                    if (explicitParam is not string and IEnumerable<object> array)
                    {
                        foreach (var item in array)
                        {
                            yield return item;
                        }
                    }

                }
            } 
            current = current is Element e ? e.Parent: null;
        }
    }
}