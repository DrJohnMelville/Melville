using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Melville.INPC;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class DelegationRequestParser
{
    private readonly bool useExplicit;
    private readonly string postProcessName;
    private readonly SemanticModel semanticModel;
    private readonly Accessibility visibility;
    private readonly IMethodNamer namer;
    public DelegationRequestParser(bool useExplicit, string postProcessName, SemanticModel semanticModel,
        Accessibility visibility, IMethodNamer namer)
    {
        this.useExplicit = useExplicit;
        this.postProcessName = postProcessName;
        this.semanticModel = semanticModel;
        this.visibility = visibility;
        this.namer = namer;
    }

    public IDelegatedMethodGenerator ParseFromMethod(IMethodSymbol symbol, SyntaxNode location) =>
        IsValidDelegatingMethod(symbol)
            ? Create(symbol.ReturnType, $"this.{symbol.Name}()", symbol, symbol.ContainingType)
            : new ErrorMethodGenerator(location, "Dele002", "Invalid Delegation method",
                $"Can only delegate to a non-void returning method with no parameters");

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.All(i=>i.IsOptional) && !symbol.ReturnsVoid;
    
    public IDelegatedMethodGenerator ParseFromProperty(IPropertySymbol ps) =>
        Create(ps.Type, $"this.{ps.Name}", ps, ps.ContainingType);

    public IDelegatedMethodGenerator ParseFromField(IFieldSymbol symbol) => 
        Create(symbol.Type, $"this.{symbol.Name}", symbol, symbol.ContainingType);

    public IDelegatedMethodGenerator ParseFromType(ITypeSymbol symbol) =>
        Create(symbol, "this", symbol, symbol);


    private IDelegatedMethodGenerator Create(
        ITypeSymbol typeToImplement, string methodPrefix, ISymbol targetSymbol, ITypeSymbol targetType)
    {
        var isMixIn = IsMixIn(targetType, typeToImplement);
        var wrappingStrategy = CreateWrappingStrategy(targetType, isMixIn);

        var options = new DelegationOptions(
            typeToImplement, targetSymbol, methodPrefix, wrappingStrategy, visibility, namer);

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
                new MixinClassGenerator(options),
            (TypeKind.Interface, false, true) =>
                new InterfaceMixinGenerator(options),
            (TypeKind.Interface, true, _) =>
                new ExplicitMethodGenerator(options),
                // new ExplicitMethodGenerator( typeToImplement, methodPrefix,
                //     typeToImplement.FullyQualifiedName() + ".", targetSymbol, wrappingStrategy),
            (TypeKind.Interface, _, _) => new InterfaceMethodGenerator(options),
            (TypeKind.Class, _, _) => new BaseClassMethodGenerator(options),

            _ => new ErrorMethodGenerator(SymbolLocation(typeToImplement),
                "Dele001", "Invalid Delegation target",
                $"Do not know how to generate delegating methods for a {typeToImplement}")
        };
    }

    private static SyntaxNode SymbolLocation(ISymbol typeToImplement) => 
        typeToImplement.DeclaringSyntaxReferences.First().GetSyntax();

    private bool IsMixIn(ITypeSymbol typeHostingMembers, ITypeSymbol typeToImplement) =>
        !typeHostingMembers.Interfaces
            .Concat(typeHostingMembers.AllBases())
            .Any(
                i => SymbolEqualityComparer.Default.Equals(i, typeToImplement));

    private IMethodWrappingStrategy CreateWrappingStrategy(ITypeSymbol newMethodHostType, bool isMixIn) =>
        string.IsNullOrEmpty(postProcessName) ?
            NoMethodMapping.Instance: 
            PickStrategyByInheritenceType(newMethodHostType, isMixIn);

    private UnrestrictedWrappingStrategy PickStrategyByInheritenceType(ITypeSymbol newMethodHostType, bool isMixIn) =>
        isMixIn?
            new UnrestrictedWrappingStrategy(postProcessName, newMethodHostType, semanticModel):
            new RestrictedWrappingStrategy(postProcessName, newMethodHostType, semanticModel);
}