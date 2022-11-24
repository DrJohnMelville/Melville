using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public readonly struct DelegationRequestParser
{
    private readonly bool useExplicit;

    public DelegationRequestParser(bool useExplicit)
    {
        this.useExplicit = useExplicit;
    }

    public IDelegatedMethodGenerator ParseFromMethod(IMethodSymbol symbol, SyntaxNode location) =>
        IsValidDelegatingMethod(symbol)
            ? Create(symbol.ReturnType, $"this.{symbol.Name}()", 
                symbol.ContainingType)
            : new InvalidTargetMethodGenerator(location);

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;
    
    public IDelegatedMethodGenerator ParseFromProperty(IPropertySymbol ps) =>
        Create(ps.Type, $"this.{ps.Name}", ps.ContainingType);

    public IDelegatedMethodGenerator ParseFromField(IFieldSymbol symbol) => 
        Create(symbol.Type, $"this.{symbol.Name}", symbol.ContainingType);

    private IDelegatedMethodGenerator Create(
        ITypeSymbol targetType, string methodPrefix, ITypeSymbol parent) =>
        (targetType.TypeKind, useExplicit) switch
        {
            (TypeKind.Interface, true) =>
                new ExplicitMethodGenerator(targetType, methodPrefix,
                    targetType.FullyQualifiedName() + ".", parent),
            (TypeKind.Interface, _) => new InterfaceMethodGenerator(targetType, methodPrefix, parent),
            (TypeKind.Class, _) => new BaseClassMethodGenerator(targetType, methodPrefix, parent),
            _ => new InvalidParentMethodGenerator(targetType,
                parent.DeclaringSyntaxReferences.First().GetSyntax())
        };
}