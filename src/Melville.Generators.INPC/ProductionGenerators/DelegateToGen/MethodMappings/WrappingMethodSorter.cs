using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public readonly struct WrappingMethodSorter
{
    private readonly ITypeSymbol type;
    private readonly string name;

    public WrappingMethodSorter(ITypeSymbol type, string name)
    {
        this.type = type;
        this.name = name;
    }

    public IList<IMethodSymbol> NonVoidMap()=> CandidateMappingMethods(IsValidNonvoidWrapper).ToList();
    public ITypeSymbol? VoidMap() => CandidateMappingMethods(ValidVoidWrapper).FirstOrDefault()?.ReturnType;

    private IEnumerable<IMethodSymbol> CandidateMappingMethods(Func<IMethodSymbol, bool> predicate)
    {
        var captureName = name;
        return type.GetMembers()
            .OfType<IMethodSymbol>()
            .Where(i => i.Name.Equals(captureName, StringComparison.Ordinal))
            .Where(predicate);
    }

    private bool ValidVoidWrapper(IMethodSymbol i) =>
        !i.IsGenericMethod && i.Parameters.All(j => j.IsOptional);

    private static bool IsValidNonvoidWrapper(IMethodSymbol i) =>
        i.Parameters.Length > 0 && 
        OnlyOneParameterRequired(i) &&
        IsConcreteOrValidGeneric(i);

    private static bool OnlyOneParameterRequired(IMethodSymbol i) => 
        i.Parameters.Skip(1).All(j => j.IsOptional);

    private static bool IsConcreteOrValidGeneric(IMethodSymbol symbol) =>
        !symbol.IsGenericMethod || FirstParameterIsGenericType(symbol);

    private static bool FirstParameterIsGenericType(IMethodSymbol method) =>
        method.TypeParameters.Length == 1 &&
        SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, method.TypeParameters[0]);

}