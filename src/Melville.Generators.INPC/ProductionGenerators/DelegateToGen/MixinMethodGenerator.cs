using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class MixinMethodGenerator : BaseClassMethodGenerator
{
    public MixinMethodGenerator(ITypeSymbol targetType, string methodPrefix, ISymbol targetSymbol) : 
        base(targetType, methodPrefix, targetSymbol)
    {
    }

    protected override string MemberDeclarationPrefix() => "public ";

    protected override bool IsForwardableSymbol(ISymbol i) =>
        i.DeclaredAccessibility is Accessibility.Public && !i.IsStatic;
}