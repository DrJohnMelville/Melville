using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodMappings;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public readonly struct DelegationRequestParser
{
    private readonly bool useExplicit;
    private readonly string postProcessName;
    private readonly SemanticModel semanticModel;

    public DelegationRequestParser(bool useExplicit, string postProcessName, SemanticModel semanticModel)
    {
        this.useExplicit = useExplicit;
        this.postProcessName = postProcessName;
        this.semanticModel = semanticModel;
    }

    public IDelegatedMethodGenerator ParseFromMethod(IMethodSymbol symbol, SyntaxNode location) =>
        IsValidDelegatingMethod(symbol)
            ? Create(symbol.ReturnType, $"this.{symbol.Name}()", symbol)
            : new ErrorMethodGenerator(location, "Dele002", "Invalid Delegation method",
                $"Can only delegate to a non-void returning method with no parameters");

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;
    
    public IDelegatedMethodGenerator ParseFromProperty(IPropertySymbol ps) =>
        Create(ps.Type, $"this.{ps.Name}", ps);

    public IDelegatedMethodGenerator ParseFromField(IFieldSymbol symbol) => 
        Create(symbol.Type, $"this.{symbol.Name}", symbol);

    private IDelegatedMethodGenerator Create(
        ITypeSymbol typeToImplement, string methodPrefix, ISymbol targetSymbol)
    {
        var isMixIn = IsMixIn(targetSymbol.ContainingType, typeToImplement);
        var wrappingStrategy = CreateWrappingStrategy(targetSymbol.ContainingType);
        return (typeToImplement.TypeKind, useExplicit, isMixIn) switch
        {
            (not TypeKind.Interface, true, _) =>
                new ErrorMethodGenerator(SymbolLocation(targetSymbol),
                    "Dele004", "Explicit implementation of non interface",
                    "Only interfaces can be explicitly implemented"),
            (_, true, true) =>
                new ErrorMethodGenerator(SymbolLocation(targetSymbol),
                    "Dele003", "Inconsistent Options",
                    "To use explicit implementation the host class must implement the target interface"),
            (TypeKind.Class or TypeKind.Struct, false, true) =>
                new MixinClassGenerator(typeToImplement, methodPrefix, targetSymbol, wrappingStrategy),
            (TypeKind.Interface, false, true) =>
                new InterfaceMixinGenerator(typeToImplement, methodPrefix, targetSymbol, wrappingStrategy),
            (TypeKind.Interface, true, _) =>
                new ExplicitMethodGenerator(typeToImplement, methodPrefix,
                    typeToImplement.FullyQualifiedName() + ".", targetSymbol, wrappingStrategy),
            (TypeKind.Interface, _, _) => new InterfaceMethodGenerator(
                typeToImplement, methodPrefix, targetSymbol, wrappingStrategy),
            (TypeKind.Class, _, _) => new BaseClassMethodGenerator(
                typeToImplement, methodPrefix, targetSymbol, wrappingStrategy),
            _ => new ErrorMethodGenerator(SymbolLocation(typeToImplement),
                "Dele001", "Invalid Delegation target",
                $"Do not know how to generate delegating methods for a {typeToImplement}")
        };
    }

    private static SyntaxNode SymbolLocation(ISymbol typeToImplement) => 
        typeToImplement.DeclaringSyntaxReferences.First().GetSyntax();

    private bool IsMixIn(INamedTypeSymbol typeHostingMembers, ITypeSymbol typeToImplement) =>
        !typeHostingMembers.Interfaces
            .Concat(typeHostingMembers.AllBases())
            .Any(
                i => SymbolEqualityComparer.Default.Equals(i, typeToImplement));

    private IMethodWrappingStrategy CreateWrappingStrategy(INamedTypeSymbol newMethodHostType) =>
        string.IsNullOrEmpty(postProcessName) ?
            NoMethodMapping.Instance: 
            new UnrestrictedWrappingStrategy(postProcessName, newMethodHostType, semanticModel);
}