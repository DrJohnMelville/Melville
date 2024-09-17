using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Melville.INPC;

namespace Melville.MVVM.Maui.Commands;

public static class InheritedCommandFactory
{
    public static CommandBase Create(Delegate action)
    {
        var parameters = action.Method.GetParameters();
        var withIoc = parameters.Any(i => i.GetCustomAttribute<FromServicesAttribute>() != null);
        return withIoc?new InternalOrIocCommand(action): new InheritedCommand(action);
    }
}


public interface IIocContextFactory
{
    IIocContext CreateContext();
}

public interface IIocContext: IDisposable{
    object GetObject(Type type, object? parameter);
}

[StaticSingleton]
public partial class InvalidIocContext : IIocContext, IIocContextFactory
{
    public void Dispose()
    {
    }

    public object GetObject(Type type, object? parameter)
    {
        throw new NotSupportedException("No Ioc Context specified.");
    }

    public IIocContext CreateContext() => this;
}

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class FromServicesAttribute : Attribute
{
}