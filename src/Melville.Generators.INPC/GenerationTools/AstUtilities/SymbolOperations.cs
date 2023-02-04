using System;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class SymbolOperations
{
    public static string AccessDeclaration(this ISymbol sym) => sym.DeclaredAccessibility.AccessDeclaration();
    public static string AccessDeclaration(this Accessibility sym) => sym switch
    {
        Accessibility.NotApplicable => throw new NotSupportedException("Unknown Access"),
        Accessibility.Private => "private",
        Accessibility.ProtectedAndInternal => "protected private",
        Accessibility.Protected => "protected",
        Accessibility.Internal => "internal",
        Accessibility.ProtectedOrInternal => "protected internal",
        _ => "public",
    };
}