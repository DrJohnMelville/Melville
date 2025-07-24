using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Melville.INPC;
using Microsoft.Extensions.DependencyInjection;

namespace Melville.MVVM.Maui.Commands;

public static class InheritedCommandFactory
{
    public static CommandBase Create(Delegate action)
    {
        var parameters = action.Method.GetParameters();
        var withIoc = parameters.Any(i => i.GetCustomAttribute<FromServicesAttribute>() != null);
        return withIoc?new InheritedOrIocCommand(action): new InheritedCommand(action);
    }
}

[StaticSingleton]
public partial class InvalidIocContext : 
    IServiceScopeFactory, IServiceScope, IServiceProvider
{
    public IServiceScope CreateScope() => this;

    public void Dispose()
    {
    }

    public IServiceProvider ServiceProvider => this;
    public object? GetService(Type serviceType)
    {
        throw new NotSupportedException("Need to register a real IOC container");
    }
}

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class FromServicesAttribute : Attribute
{
}