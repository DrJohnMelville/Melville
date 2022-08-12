using System.IO;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;

public class StaticSingletonLabeledMember: ILabeledMembersSyntaxModel
{
    private TypeDeclarationSyntax typeSyntax;

    public StaticSingletonLabeledMember(TypeDeclarationSyntax typeSyntax)
    {
        this.typeSyntax = typeSyntax;
    }

    public void AddMember(SyntaxNode declSyntax) { }

    public ILabeledMembersSemanticModel AddSemanticInfo(SemanticModel semanticModel)
    {
        return new StaticSingletonCodeGenerator(typeSyntax,
            GetTypeSymbol(semanticModel));
    }

    private INamedTypeSymbol GetTypeSymbol(SemanticModel semanticModel) =>
        semanticModel.GetDeclaredSymbol(typeSyntax) as INamedTypeSymbol ??
        throw new InvalidDataException("[StaticSingeton] must decorate a type");
}