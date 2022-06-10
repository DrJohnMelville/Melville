using System;
using System.Collections.Generic;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class StringChangers
{

    public static string AsInstanceSymbol(this string s) => 
        s.WithFirstLowerCase().AvoidCSharpKeywords();
    public static string WithFirstLowerCase(this string s) =>
        s.Length > 0 && char.IsUpper(s[0]) ? char.ToLower(s[0]) + s.Substring(1) : s;

    public static string AvoidCSharpKeywords(this string s) =>
        keywords.Contains(s) ? "@" + s : s;
    private static readonly HashSet<string> keywords = new()
    {
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while",
    };
}