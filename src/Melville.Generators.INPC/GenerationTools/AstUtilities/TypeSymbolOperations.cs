using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class TypeSymbolOperations
{
    public static IEnumerable<ITypeSymbol> AllBases(this ITypeSymbol? typeSymbol)
    {
        for (var i = typeSymbol?.BaseType; i != null; i = i.BaseType) yield return i;
    }

    public static string FullyQualifiedName(this ITypeSymbol symbol) => symbol.ToString();
    public static bool IsVoid(this ITypeSymbol symbol) => symbol.SpecialType == SpecialType.System_Void;

    public static bool HasMethod(
        this ITypeSymbol symbol, ITypeSymbol? returnType, string name, params ITypeSymbol[] parameters) =>
        HasMethodImpl(symbol, returnType?.FullyQualifiedName(), name,
            parameters.Select(i => FullyQualifiedName(i)).ToArray());

    public static bool HasMethod(
        this ITypeSymbol symbol, Type? returnType, string name, params Type[] parameters) =>
        HasMethodImpl(symbol, returnType?.FullyQualifiedName(), name,
            parameters.Select(i => i.FullyQualifiedName()).ToArray());

    private static bool HasMethodImpl(
        this ITypeSymbol? symbol, string? returnType, string name, params string[] paramTypes)
    {
        if (symbol == null) return false;
        return HasLocalMethod(symbol, returnType, name, paramTypes) ||
               HasMethodImpl(symbol.BaseType, returnType, name, paramTypes);
    }
        
    private static bool HasLocalMethod(
        this ITypeSymbol symbol, string? returnType, string name, params string[] parameters) =>
        symbol.GetMembers().OfType<IMethodSymbol>().Any(m =>
            MethodMatches(returnType, name, parameters, m));

    private static bool MethodMatches(
        string? returnType, string name, string[] parameters, IMethodSymbol m) =>
        ReturnTypeMatches(returnType, m) &&
        name.Equals(m.Name, StringComparison.Ordinal) &&
        ParametersMatch(parameters, m.Parameters);

    private static bool ParametersMatch(string[] expected, ImmutableArray<IParameterSymbol> found) =>
        (expected.Length == found.Length)
        && expected.Zip(found, (i, j) => TypeMatches(j.Type, i)).All(i=>i);

    private static bool ReturnTypeMatches(string? returnType, IMethodSymbol m) =>
        returnType == null ? m.ReturnsVoid :
            TypeMatches(m.ReturnType, returnType);

    private static bool TypeMatches(ITypeSymbol symbol, string type) => 
        symbol.FullyQualifiedName().Equals(type);

    public static IEnumerable<ISymbol> MembersLabeledWith(this ITypeSymbol symbol, string attrName) =>
        symbol.GetMembers().Where(i => i.GetAttributes().FilterToAttributeType(attrName).Any());

    public static ITypeSymbol ThisOrContainingTypeSymbol(this ISymbol sym) =>
        sym as ITypeSymbol ?? sym.ContainingType;
}