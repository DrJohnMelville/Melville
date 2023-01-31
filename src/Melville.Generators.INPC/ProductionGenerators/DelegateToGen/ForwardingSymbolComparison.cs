using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class ForwardingSymbolComparison
{
    public static bool HasSimilarSymbol(this ITypeSymbol host, ISymbol member) =>
        host.GetMembers()
            .Any(i => SimilarSymbolEqualityComparer.Instance.Equals(i, member));
}

public class SimilarSymbolEqualityComparer : IEqualityComparer<ISymbol>
{
    public static readonly SimilarSymbolEqualityComparer Instance = new();
    public bool Equals(ISymbol a, ISymbol b) =>
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
            if (!SameSymbol(a[i].Type, b[i].Type)) return false;
        }

        return true;
    }

    private static bool SameSymbol(ISymbol child, ISymbol par) => SymbolEqualityComparer.Default.Equals(child, par);

    public int GetHashCode(ISymbol obj)
    {
        var code = new HashCode();
        code.Add(obj.Kind);
        code.Add(obj.Name);
        if (obj is IMethodSymbol method)
        {
            foreach (var parameter in method.Parameters)
            {
                code.Add(parameter.Type);
            }
        }

        return code.Code;
    }

    private ref struct HashCode
    {
        public int Code = 1000;

        public HashCode()
        { }

        public void Add<T>(T item)
        {
            unchecked
            {
                Code = Code * 1009 + item?.GetHashCode()??0;
            }
        }
    }
}