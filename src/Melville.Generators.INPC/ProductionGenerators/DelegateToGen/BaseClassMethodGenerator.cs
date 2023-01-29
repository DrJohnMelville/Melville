using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class BaseClassMethodGenerator : DelegatedMethodGenerator
{
    public BaseClassMethodGenerator(ITypeSymbol targetType, string methodPrefix, ISymbol parentSymbol) : 
        base(targetType, methodPrefix, parentSymbol)
    {
    }

    protected override string MemberDeclarationPrefix() => "public override ";

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() => 
        AllTypes()
            .SelectMany(i=>i.GetMembers())
            .Distinct(SymbolEqualityComparer.Default)
            .Where(IsForwardableSymbol);

    private IEnumerable<ITypeSymbol> AllTypes()
    {
        var curentType = TargetType;
        while (curentType != null)
        {
            yield return TargetType;
            foreach (var currentInterface in TargetType.Interfaces)
            {
                yield return currentInterface;
            }

            curentType = curentType.BaseType;
        }
    }

    protected virtual bool IsForwardableSymbol(ISymbol i) => 
        (i.IsVirtual || i.IsAbstract) && i.DeclaredAccessibility == Accessibility.Public;

    protected override bool ImplementationMissing(ISymbol sym) => 
        !ParentSymbol.GetMembers().Any(i => i.IsOverride && i.Name == sym.Name && CompareArgumentLists(i, sym));

    private bool CompareArgumentLists(ISymbol a, ISymbol b)
    {
        if (!(a is IMethodSymbol parent && b is IMethodSymbol sym)) return true;
        return SameSymbol(parent.ReturnType, sym.ReturnType) &&
               (parent.Parameters.Length == sym.Parameters.Length) &&
               parent.Parameters.Zip(sym.Parameters, (x, y) => SameSymbol(x.Type, y.Type)).All(i => i);
    }
    private static bool SameSymbol(ISymbol child, ISymbol par) => SymbolEqualityComparer.Default.Equals(child, par);
}