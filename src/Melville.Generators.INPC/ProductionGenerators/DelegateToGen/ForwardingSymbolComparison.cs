using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class ForwardingSymbolComparison
{
    public static bool HasSimilarSymbol(this ITypeSymbol host, ISymbol member) =>
        host.GetMembers().Any(i => IsSimilarSymbolForForwarding(i, member));

    public static bool IsSimilarSymbolForForwarding(ISymbol a, ISymbol b) =>
        a.Kind == b.Kind &&
        a.Name.Equals(b.Name, StringComparison.Ordinal) &&
        CompareOverloads(a, b);

    private static bool CompareOverloads(ISymbol a, ISymbol b)
    {
        if (!(a is IMethodSymbol aMethod && b is IMethodSymbol bMethod)) return true;
        if (!CompareArgumentLists(aMethod.Parameters, bMethod.Parameters)) return false;
        return true;
    }

    private static bool CompareArgumentLists(ImmutableArray<IParameterSymbol> a, ImmutableArray<IParameterSymbol> b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
        {
            if (!SameSymbol(a[i], b[i])) return false;
        }

        return true;
    }

    private static bool SameSymbol(ISymbol child, ISymbol par) => SymbolEqualityComparer.Default.Equals(child, par);
}