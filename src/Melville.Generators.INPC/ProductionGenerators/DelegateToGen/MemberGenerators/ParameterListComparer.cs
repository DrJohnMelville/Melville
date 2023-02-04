using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

public static class ParameterListComparer
{
    public static bool IsEquivilentTo(
        this ImmutableArray<IParameterSymbol> parameters, ImmutableArray<IParameterSymbol> other) =>
        parameters.Length == other.Length &&
        parameters.Zip(other, ParametersHaveSameType).All(i => i);

    private static bool ParametersHaveSameType(IParameterSymbol a, IParameterSymbol b) => 
        SymbolEqualityComparer.Default.Equals(a.Type, b.Type);
}