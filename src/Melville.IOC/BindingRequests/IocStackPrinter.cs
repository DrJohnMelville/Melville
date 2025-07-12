using System;
using System.Text;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

public static class IocStackPrinter
{
    public static string IocStackTrace(this IIocService top, IBindingRequest? query)
    {
        var sb = new StringBuilder();
        top.PrintTraceTo(sb, query);
        return sb.ToString();
    }

    public static void PrintTraceTo(this IIocService top, StringBuilder sb, IBindingRequest? query)
    {
        foreach (var service in top.ScopeList())
        {
            PrintSingleLine(service, sb, query);
        }
    }

    private static void PrintSingleLine(IIocService service, StringBuilder sb, IBindingRequest? query)
    {
        sb.AppendLine($"{service.GetType().PrettyName()}(0x{service.GetHashCode():X}) {ScopeDot(service, query)}");
    }

    private static string ScopeDot(IIocService service, IBindingRequest? query)
    {
        try
        {
            return query is not null && service is IScope scope &&
                   scope.TryGetValue(query, out _)
                ? " -- Satisfies Request"
                : "";
        }
        catch (IocException e)
        {
            return " --" + e.Message;
        }
    }
}