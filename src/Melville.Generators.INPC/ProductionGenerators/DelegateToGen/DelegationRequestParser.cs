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
                symbol)
            : new InvalidTargetMethodGenerator(location);

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;
    
    public IDelegatedMethodGenerator ParseFromProperty(IPropertySymbol ps) =>
        Create(ps.Type, $"this.{ps.Name}", ps);

    public IDelegatedMethodGenerator ParseFromField(IFieldSymbol symbol) => 
        Create(symbol.Type, $"this.{symbol.Name}", symbol);

    private IDelegatedMethodGenerator Create(
        ITypeSymbol targetType, string methodPrefix, ISymbol target) =>
        (targetType.TypeKind, useExplicit) switch
        {
            (TypeKind.Interface, true) =>
                new ExplicitMethodGenerator(targetType, methodPrefix,
                    targetType.FullyQualifiedName() + ".", target),
            (TypeKind.Interface, _) => new InterfaceMethodGenerator(targetType, methodPrefix, target),
            (TypeKind.Class, _) => new BaseClassMethodGenerator(targetType, methodPrefix, target),
            _ => new InvalidParentMethodGenerator(targetType,
                target.DeclaringSyntaxReferences.First().GetSyntax())
        };
}