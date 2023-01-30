using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class MixinMethodGenerator : BaseClassMethodGenerator
{
    public MixinMethodGenerator(ITypeSymbol targetType, string methodPrefix, ISymbol targetSymbol) :
        base(targetType, methodPrefix, targetSymbol)
    {
    }

    protected override string MemberDeclarationPrefix(ISymbol sym) => sym.AccessDeclaration() + " ";

    protected override bool IsForwardableSymbol(ISymbol i) =>
        !i.IsStatic && CanSeeSumbol(i, false);

    private bool CanSeeSumbol(ISymbol methodToGenerate, bool isDescendant) =>
        (methodToGenerate.DeclaredAccessibility, SameAssembly(), isDescendant) switch 
        {
            (Accessibility.Public, _, _) => true,
            (Accessibility.Internal, true, _) => true,
            (Accessibility.ProtectedAndInternal, true, true) => true,
            (Accessibility.ProtectedOrInternal, true, _) => true,
            (Accessibility.ProtectedOrInternal, _, true) => true,
            (Accessibility.Protected, _, true) => true,
            _=>false,
        };

    private bool SameAssembly() =>
        SymbolEqualityComparer.Default.Equals(GeneratedMethodSourceSymbol, GeneratedMethodHostSymbol);
}
