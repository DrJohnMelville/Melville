using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;

public class MixinClassGenerator : BaseClassMethodGenerator
{
    public MixinClassGenerator(DelegationOptions options) : base(options)
    {
    }

    protected override string MemberDeclarationPrefix(Accessibility sym) => sym.AccessDeclaration() + " ";

    protected override bool IsForwardableSymbol(ISymbol i) => !i.IsStatic && CanSeeSymbol(i);
    protected override bool HostDescendsFromSource => false;
}
