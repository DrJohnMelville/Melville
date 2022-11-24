using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public readonly struct DelegationRequestParser
{
    private readonly SemanticModel model;
    private readonly SyntaxNode member;
    private readonly bool useExplicit;
    private readonly ITypeSymbol parent;

    public DelegationRequestParser(SemanticModel model, SyntaxNode member, bool useExplicit)
    {
        this.model = model;
        this.member = member;
        this.useExplicit = useExplicit;
        parent = GetParent(model, member);
    }

    public IDelegatedMethodGenerator? ParseItem() =>
        member switch
        {
            FieldDeclarationSyntax fs => ParseFromField(fs),
            PropertyDeclarationSyntax pds => ParseFromProperty(pds),
            MethodDeclarationSyntax mds => ParseFromMethod(mds),
            _ => throw new InvalidProgramException("This is not a valid delegation target")
        };

    private static ITypeSymbol GetParent(SemanticModel model, SyntaxNode member) =>
        member.Parent is not { } parentSyntax ||
        model.GetDeclaredSymbol(parentSyntax) is not ITypeSymbol parent
            ? throw new InvalidProgramException("Cannot find parent of delegated item")
            : parent;

    private IDelegatedMethodGenerator? ParseFromMethod(MethodDeclarationSyntax mds) =>
        model.GetDeclaredSymbol(mds) is IMethodSymbol symbol && IsValidDelegatingMethod(symbol)
            ? DelegateMethodGeneratorFactory.Create(symbol.ReturnType, $"this.{symbol.Name}()", parent, useExplicit)
            : null;

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;

    private IDelegatedMethodGenerator? ParseFromProperty(PropertyDeclarationSyntax pds) => 
        model.GetDeclaredSymbol(pds) is IPropertySymbol ps ? 
            DelegateMethodGeneratorFactory.Create(ps.Type, $"this.{ps.Name}", parent, useExplicit) : 
            null;

    private IDelegatedMethodGenerator? ParseFromField(FieldDeclarationSyntax fs) =>
        model.GetDeclaredSymbol(FirstVariableDecl(fs)) is IFieldSymbol symbol?
            DelegateMethodGeneratorFactory.Create(symbol.Type, $"this.{symbol.Name}", parent, useExplicit):
            null;

    private VariableDeclaratorSyntax FirstVariableDecl(FieldDeclarationSyntax fs) => 
        fs.Declaration.Variables.First();
}