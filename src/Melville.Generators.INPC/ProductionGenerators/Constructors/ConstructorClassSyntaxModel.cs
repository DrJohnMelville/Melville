using System.IO;
using System.Runtime.CompilerServices.ProductionGenerators.Constructors;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.Constructors;

public class ConstructorClassSyntaxModel : ILabeledMembersSyntaxModel
{
    private readonly TypeDeclarationSyntax classDeclaration;

    public ConstructorClassSyntaxModel(TypeDeclarationSyntax type)
    {
        classDeclaration = type;
    }

    public void AddMember(SyntaxNode declSyntax)
    {
    }

    public ILabeledMembersSemanticModel AddSemanticInfo(SemanticModel semanticModel) => 
        new ConstructorCodeGeneratorFactory(SymbolForTargetClass(semanticModel) ).Create(classDeclaration);

    private INamedTypeSymbol SymbolForTargetClass(SemanticModel semanticModel) =>
        semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol??
        throw new InvalidDataException("Must have named symbol fot class");
}