using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class MixinClassGenerator : BaseClassMethodGenerator
{
    public MixinClassGenerator(ITypeSymbol sourceType, string methodPrefix, ISymbol targetSymbol, IMethodWrappingStrategy wrappingStrategy) :
        base(sourceType, methodPrefix, targetSymbol, wrappingStrategy)
    {
    }

    protected override string MemberDeclarationPrefix(Accessibility sym) => sym.AccessDeclaration() + " ";

    protected override bool IsForwardableSymbol(ISymbol i) => !i.IsStatic && CanSeeSymbol(i);
    protected override bool HostDescendsFromSource => false;
}
