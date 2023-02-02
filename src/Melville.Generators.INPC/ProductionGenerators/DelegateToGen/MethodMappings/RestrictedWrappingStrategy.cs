using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public class RestrictedWrappingStrategy : UnrestrictedWrappingStrategy
{
    public RestrictedWrappingStrategy(string name, ITypeSymbol type, SemanticModel semanticModel) : 
        base(name, type, semanticModel)
    {
    }

    protected override ITypeSymbol? MapType(ITypeSymbol typeSymbol) => 
        CanReplaceInInterface(typeSymbol, base.MapType(typeSymbol)) ? typeSymbol : null;

    private bool CanReplaceInInterface(ITypeSymbol typeSymbol, ITypeSymbol? replacedWith) =>
        IsAssignableTo(replacedWith, typeSymbol) ||
        CanIgnoreResultAndReturnVoid(replacedWith, typeSymbol);

    private static bool CanIgnoreResultAndReturnVoid(ITypeSymbol? replacedWith, ITypeSymbol typeSymbol) => 
        typeSymbol.IsVoid() && replacedWith != null;
}