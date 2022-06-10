using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class TypeOperations {
    public static string FullyQualifiedName(this Type type) => 
        type.IsConstructedGenericType ? GenericTypeName(type) : NongenericTypeName(type);

    private static string GenericTypeName(Type type) => 
        $"{type.Namespace}.{ExtractGenericName(type)}<{AllNames(type.GenericTypeArguments)}>";

    private static string AllNames(Type[] types) => 
        string.Join(", ", types.Select(i => FullyQualifiedName(i)));

    private static Regex GetTypeName = new (@"^\w+");
    private static string ExtractGenericName(Type type) => 
        GetTypeName.Match(type.Name).Value;

    private static string NongenericTypeName(Type type)
    {
        return type.ToString() switch
        {
            "System.Int32" => "int",
            "System.UInt32" => "uint",
            "System.Int64" => "long",
            "System.UInt64" => "ulong",
            "System.Int16" => "short",
            "System.UInt16" => "ushort",
            "System.Byte" => "byte",
            "System.SByte" => "sbyte",
            "System.Char" => "char",
            "System.Single" => "float",
            "System.Double" => "double",
            "System.Object" => "object",
            "System.String" => "string",
            _ => type.ToString()
        };
    }
}