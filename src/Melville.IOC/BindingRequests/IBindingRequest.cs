using System;
using System.Reflection;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies;

namespace Melville.IOC.BindingRequests;

public interface IBindingRequest
{
    IBindingRequest? Parent { get; }
    Type DesiredType { get; }
    Type? TypeBeingConstructed { get; }
    string TargetParameterName { get; }
    IIocService IocService { get; }
    bool IsCancelled { get; set; }

    bool HasDefaultValue(out object? value)
    {
        value = null;
        return false;
    }

    object?[] ArgumentsFromChild { get; set; }
    object?[] ArgumentsFromParent { get; }
    string Trace { get; }

    CreateSingletonRequest? SingletonRequestParent { get; }
    IRegisterDispose DisposeScope { get; }
}

public static class BindingRequestExtensions
{
    public static IBindingRequest CreateSubRequest(this IBindingRequest req, ParameterInfo info)=> new ParameterBindingRequest(info, req);
    public static IBindingRequest CreateSubRequest(this IBindingRequest req, Type type)=> new TypeChangeBindingRequest(req, type);
    public static IBindingRequest CreateSubRequest(this IBindingRequest req, Type type, params object[] parameters)=> new ParameterizedRequest(req, type, parameters);
    public static IBindingRequest Clone(this IBindingRequest req) => new ClonedBindingRequest(req);
}