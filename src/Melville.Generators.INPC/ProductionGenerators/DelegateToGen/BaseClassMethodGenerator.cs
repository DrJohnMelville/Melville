using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class BaseClassMethodGenerator : DelegatedMethodGenerator
{
    public BaseClassMethodGenerator(ITypeSymbol targetType, string methodPrefix, ITypeSymbol parentSymbol, SyntaxNode location) : 
        base(targetType, methodPrefix, parentSymbol, location)
    {
    }

    protected override string MemberDeclarationPrefix() => "public override ";

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() => 
        TargetType.GetMembers().Where(IsForwardableSymbol);

    private bool IsForwardableSymbol(ISymbol i)
    {
        return (i.IsVirtual || i.IsAbstract) && i.DeclaredAccessibility == Accessibility.Public;
    }

    protected override bool ImplementationMissing(ISymbol sym)
    {
        return !parentSymbol.GetMembers().Any(i => i.IsOverride && i.Name == sym.Name && CompareArgumentLists(i, sym));
    }

    private bool CompareArgumentLists(ISymbol parentSymbol, ISymbol childSymbol)
    {
        if (!(parentSymbol is IMethodSymbol parent && childSymbol is IMethodSymbol sym)) return true;
        if (!SameSymbol(parent.ReturnType, sym.ReturnType)) return false;
        if (parent.Parameters.Length != sym.Parameters.Length) return false;
        if (!parent.Parameters.Zip(sym.Parameters, (x, y) => SameSymbol(x.Type, y.Type)).All(i=>i)) return false;
        return true;
    }

    private static bool SameSymbol(ISymbol child, ISymbol par) => SymbolEqualityComparer.Default.Equals(child, par);
}