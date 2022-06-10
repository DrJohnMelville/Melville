using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public static class DelegationRequestParser
{
    public static IDelegatedMethodGenerator? ParseItem(
        SemanticModel model, SyntaxNode member) =>
        member switch
        {
            FieldDeclarationSyntax fs => ParseFromField(model, fs),
            PropertyDeclarationSyntax pds => ParseFromProperty(model, pds),
            MethodDeclarationSyntax mds => ParseFromMethod(model, mds),
            _ => throw new InvalidProgramException("This is not a valid delegation target")
        };

    private static IDelegatedMethodGenerator? ParseFromMethod(
        SemanticModel model, MethodDeclarationSyntax mds) =>
        model.GetDeclaredSymbol(mds) is IMethodSymbol symbol && IsValidDelegatingMethod(symbol)
            ? DelegatedMethodGenerator.Create(symbol.ReturnType, $"this.{symbol.Name}()", mds)
            : null;

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;

    private static IDelegatedMethodGenerator? ParseFromProperty(
        SemanticModel model, PropertyDeclarationSyntax pds) => 
        model.GetDeclaredSymbol(pds) is IPropertySymbol ps ? 
            DelegatedMethodGenerator.Create(ps.Type, $"this.{ps.Name}", pds) : 
            null;

    private static IDelegatedMethodGenerator? ParseFromField(
        SemanticModel model, FieldDeclarationSyntax fs) =>
        model.GetDeclaredSymbol(FirstVariableDecl(fs)) is IFieldSymbol symbol?
            DelegatedMethodGenerator.Create(symbol.Type, $"this.{symbol.Name}", fs):
            null;

    private static VariableDeclaratorSyntax FirstVariableDecl(FieldDeclarationSyntax fs) => 
        fs.Declaration.Variables.First();
}