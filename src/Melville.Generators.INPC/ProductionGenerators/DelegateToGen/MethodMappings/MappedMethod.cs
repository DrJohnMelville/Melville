using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;

public readonly struct MappedMethod
{
    public readonly ITypeSymbol FinalType { get; }
    public string OpenBody { get; }
    public string CloseBody { get; }

    public MappedMethod(ITypeSymbol finalType, string openBody, string closeBody)
    {
        OpenBody = openBody;
        CloseBody = closeBody;
        FinalType = finalType;
    }

    public string CastResultTo(ITypeSymbol target) =>
        SymbolEqualityComparer.Default.Equals(target, FinalType) ? "" : $"({target.FullyQualifiedName()})";
}