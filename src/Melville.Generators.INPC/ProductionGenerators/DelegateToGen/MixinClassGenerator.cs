using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class MixinClassGenerator : BaseClassMethodGenerator
{
    public MixinClassGenerator(ITypeSymbol targetType, string methodPrefix, ISymbol targetSymbol) :
        base(targetType, methodPrefix, targetSymbol)
    {
    }

    public override string MemberDeclarationPrefix(ISymbol sym) => sym.AccessDeclaration() + " ";

    protected override bool IsForwardableSymbol(ISymbol i) => !i.IsStatic && CanSeeSymbol(i);
    protected override bool HostDescendsFromSource => false;
}
