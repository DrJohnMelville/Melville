using System.Linq;
using System.Text;
using Melville.IOC.IocContainers;

namespace Melville.IOC.BindingRequests;

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
        for (var i = item; i != null; i = i.Parent)
        {
            AppendRequestLevel(sb, i);

        }
    }

    private static void AppendRequestLevel(StringBuilder sb, IBindingRequest item)
    {
        sb.AppendLine();
        sb.Append(
            $"[{ChildLevels(item)}] {RequestedTypeName(item)} ({ScopeTag(item)}, {DisposeTag(item)})");
    }

    private static int ChildLevels(IBindingRequest item)
    {
        return item.IocService.ScopeList().Count();
    }

    private static string ScopeTag(IBindingRequest request) => 
        request.IocService.ScopeList().OfType<IScope>().Any()?
       "Scoped": "No Scope";

    private static string DisposeTag(IBindingRequest br) =>
        br.IocService.ScopeList().OfType<IRegisterDispose>().FirstOrDefault() switch
        {
            null => br.IocService.AllowDisposablesInGlobalScope
                ? "Global Dispose Allowed"
                : "Global Dispose Not Allowed",
            { SatisfiesDisposeRequirement: true } => "Dispose Allowed:",
            _ => "Dispose Not Allowed"
        };
    private static string RequestedTypeName(IBindingRequest requestedType)
    {
        return requestedType.DesiredType.FullName??requestedType.DesiredType.Name;
            
    }

}