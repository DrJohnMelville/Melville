using System;
using System.Reflection;
using System.Text;
using Melville.IOC.IocContainers;

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

    object?[] ArgumentsFormChild { get; set; }
    object?[] ArgumentsFromParent { get; }
    string Trace { get; }
}

public static class BindingRequestExtensions
{
    public static IBindingRequest CreateSubRequest(this IBindingRequest req, ParameterInfo info)=> new ParameterBindingRequest(info, req);
    public static IBindingRequest CreateSubRequest(this IBindingRequest req, Type type)=> new TypeChangeBindingRequest(req, type);
    public static IBindingRequest CreateSubRequest(this IBindingRequest req, Type type, params object[] parameters)=> new ParameterizedRequest(req, type, parameters);
    public static IBindingRequest Clone(this IBindingRequest req) => new ClonedBindingRequest(req);
}

public static class RequestStackPrinter
{
    public static string ConstructFailureMessage(this IBindingRequest request)
    {
        var sb = new StringBuilder();
        sb.Append($"Cannot bind type: {request.DesiredType.Name}");
        AppendRequestList(request, sb);
        return sb.ToString();
    }

    private static void AppendRequestList(IBindingRequest? item, StringBuilder sb)
    {
        var level = 1;
        for (var i = item; i != null; i = i.Parent, level++)
        {
            AppendRequestLevel(sb, level, i);

        }
    }

    private static void AppendRequestLevel(StringBuilder sb, int level, IBindingRequest item)
    {
        sb.AppendLine();
        sb.Append($"    {level}. {RequestedTypeName(item)} -- {ScopeTag(item)}");
    }

    private static string ScopeTag(IBindingRequest request) => request.IocService.IsGlobalScope ? "No Scope" : "In Scope";
    private static string RequestedTypeName(IBindingRequest requestedType)
    {
        return requestedType.DesiredType.FullName??requestedType.DesiredType.Name;
            
    }

}
