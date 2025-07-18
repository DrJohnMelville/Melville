﻿using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ChildContainers;
using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Melville.IOC.BindingRequests;

public static class RequestStackPrinter
{
    public static string Print(this IBindingRequest request)
    {
        var sb = new StringBuilder();
        sb.Append($"Requested type: {request.DesiredType.Name}");
        AppendRequestList(request, sb);
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("Active Scopes:");
        request.IocService.PrintTraceTo(sb, request);
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
            $"[{ChildLevels(item)}, {CountIndirections(item.IocService)}] {item.GetType().PrettyName()} {RequestedTypeName(item)}");
    }

    private static int CountIndirections(IIocService itemIocService) => (itemIocService as ChildContainer)?.Depth ?? 1;

    private static int ChildLevels(IBindingRequest item)
    {
        return item.IocService.ScopeList().Count();
    }

    private static string RequestedTypeName(IBindingRequest requestedType) => 
        requestedType.DesiredType.PrettyName();

    public static string PrettyName(this Type objType)
    {

        string result = objType.Name;
        if (objType.IsGenericType)
        {
            var name = objType.Name.AsSpan(0,objType.Name.IndexOf('`'));
            var genericTypes = objType.GenericTypeArguments;
            result = $"{name}<{string.Join(",", genericTypes.Select(PrettyName))}>";
        }
        return result;
    }

}