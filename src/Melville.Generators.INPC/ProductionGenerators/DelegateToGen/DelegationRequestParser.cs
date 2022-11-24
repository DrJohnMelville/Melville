using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public readonly struct DelegationRequestParser
{
    private readonly bool useExplicit;

    public DelegationRequestParser(bool useExplicit)
    {
        this.useExplicit = useExplicit; ;
    }

    public IDelegatedMethodGenerator ParseFromMethod(IMethodSymbol symbol, SyntaxNode location) =>
        IsValidDelegatingMethod(symbol)
            ? DelegateMethodGeneratorFactory.Create(symbol.ReturnType, $"this.{symbol.Name}()", 
                symbol.ContainingType, useExplicit)
            : new InvalidTargetMethodGenerator(location);

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;
    
    public IDelegatedMethodGenerator ParseFromProperty(IPropertySymbol ps)
    {
        return DelegateMethodGeneratorFactory.Create(ps.Type, $"this.{ps.Name}", 
            ps.ContainingType, useExplicit);
    }

    public IDelegatedMethodGenerator ParseFromField(IFieldSymbol symbol) => 
        DelegateMethodGeneratorFactory.Create(symbol.Type, $"this.{symbol.Name}", 
            symbol.ContainingType, useExplicit);
}