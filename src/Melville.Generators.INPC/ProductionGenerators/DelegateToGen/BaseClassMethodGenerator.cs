using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class BaseClassMethodGenerator : DelegatedMethodGenerator
{
    public BaseClassMethodGenerator(ITypeSymbol targetType, string methodPrefix, ISymbol parentSymbol) :
        base(targetType, methodPrefix, parentSymbol)
    {
    }

    public override string MemberDeclarationPrefix(ISymbol replacedSumbol) =>
        replacedSumbol.AccessDeclaration() + " override ";

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() =>
        AllTypes()
            .SelectMany(i => i.GetMembers())
            .Distinct(SymbolEqualityComparer.Default)
            .Where(IsForwardableSymbol);

    private IEnumerable<ITypeSymbol> AllTypes() => 
        GeneratedMethodSourceSymbol.AllBases().Prepend(GeneratedMethodSourceSymbol);

    protected virtual bool IsForwardableSymbol(ISymbol i) =>
        (i.IsVirtual || i.IsAbstract) && CanSeeSymbol(i);

    protected override bool ImplementationMissing(ISymbol sym) =>
        !GeneratedMethodHostSymbol.HasSimilarSymbol(sym);
}