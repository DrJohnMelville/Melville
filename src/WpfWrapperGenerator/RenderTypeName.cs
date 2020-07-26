using System;
using System.Linq;

namespace WpfWrapperGenerator
{
    public static class RenderTypeName
    {

        public static string CSharpName(this Type? type) =>
            type switch
            {
                null => "*** Error No Type Name **",
                var t when t.IsGenericTypeDefinition => (t.FullName??"`1")[..^2],
                var t when t.IsGenericType => $"{CSharpName(t.GetGenericTypeDefinition())}<{ArgList(t)}>",
                _ => type.FullName ?? "** No full Name to Render **"
            };
        private static string ArgList(Type t) => 
            string.Join(", ", t.GetGenericArguments().Select(CSharpName));
    }
}