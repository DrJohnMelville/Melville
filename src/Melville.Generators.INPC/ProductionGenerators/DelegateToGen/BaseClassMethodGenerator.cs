using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class BaseClassMethodGenerator : DelegatedMethodGenerator
{
    public BaseClassMethodGenerator(ITypeSymbol targetType, string methodPrefix, ISymbol parentSymbol,
        IMethodWrappingStrategy wrappingStrategy) :
        base(targetType, methodPrefix, parentSymbol, wrappingStrategy)
    {
    }

    public override string MemberDeclarationPrefix(ISymbol replacedSumbol) =>
        replacedSumbol.AccessDeclaration() + " override ";

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() =>
//We are not using the SymbolEqualityComparer specifically because we want a looser comparison of the symbols
#pragma warning disable RS1024
        AllTypes()
            .Where(i=>i.SpecialType is SpecialType.None)
            .SelectMany(i => i.GetMembers())
            .Distinct(SimilarSymbolEqualityComparer.Instance)
            .Where(IsForwardableSymbol);
#pragma warning restore RS1024

    private IEnumerable<ITypeSymbol> AllTypes() => 
        GeneratedMethodSourceSymbol.AllBases().Prepend(GeneratedMethodSourceSymbol);

    protected virtual bool IsForwardableSymbol(ISymbol i) =>
        (i.IsVirtual || i.IsAbstract) && CanSeeSymbol(i);

    protected override bool ImplementationMissing(ISymbol sym) =>
        !GeneratedMethodHostSymbol.HasSimilarSymbol(sym);
}