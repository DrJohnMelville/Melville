using System.Security.Cryptography;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public record DelegationOptions(
    ITypeSymbol SourceType,
    ISymbol HostSymbol, 
    string MethodPrefix,
    IMethodWrappingStrategy WrappingStrategy,
    Accessibility DesiredAccessibility
)
{
    public ITypeSymbol HostClass => HostSymbol.ContainingType;
    public Accessibility ComputeAccessibilityFor(Accessibility source) =>
        DesiredAccessibility == Accessibility.NotApplicable ? source : DesiredAccessibility;
}