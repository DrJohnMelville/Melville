using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;

public class BaseClassMethodGenerator : ClassGenerator
{
    public BaseClassMethodGenerator(DelegationOptions options) : base(options)
    {
    }

    protected override string MemberDeclarationPrefix(Accessibility suggestedAccess) =>
        suggestedAccess.AccessDeclaration() + " ";

    protected override IEnumerable<ISymbol> MembersThatCouldBeForwarded() =>
        //We are not using the SymbolEqualityComparer specifically because we want a looser comparison of the symbols
#pragma warning disable RS1024
        AllTypes()
            .Where(i => i.SpecialType is SpecialType.None)
            .SelectMany(i => i.GetMembers())
            .Where(IsForwardableSymbol)
            .Distinct(SimilarSymbolEqualityComparer.Instance);
#pragma warning restore RS1024

    private IEnumerable<ITypeSymbol> AllTypes() =>
        SourceType.AllBases().Prepend(SourceType);

    protected virtual bool IsForwardableSymbol(ISymbol i) =>
        !i.IsStatic && CanSeeSymbol(i);
}