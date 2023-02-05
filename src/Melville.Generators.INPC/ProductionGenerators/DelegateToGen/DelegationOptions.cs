using System.Security.Cryptography;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public record DelegationOptions(
    ITypeSymbol SourceType,
    ISymbol HostSymbol, 
    string MethodPrefix,
    IMethodWrappingStrategy WrappingStrategy,
    Accessibility DesiredAccessibility,
    IMethodNamer Namer
)
{
    public ITypeSymbol HostClass => HostSymbol is ITypeSymbol ts?ts: HostSymbol.ContainingType;
    public Accessibility ComputeAccessibilityFor(Accessibility source) =>
        DesiredAccessibility == Accessibility.NotApplicable ? source : DesiredAccessibility;

    public bool IsSelfGeneration() => SymbolEqualityComparer.Default.Equals(SourceType, HostClass);
}