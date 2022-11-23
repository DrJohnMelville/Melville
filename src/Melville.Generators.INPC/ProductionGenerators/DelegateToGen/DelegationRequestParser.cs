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
            FieldDeclarationSyntax fs => ParseFromField(model, fs, GetParent(model, member)),
            PropertyDeclarationSyntax pds => ParseFromProperty(model, pds, GetParent(model, member)),
            MethodDeclarationSyntax mds => ParseFromMethod(model, mds, GetParent(model, member)),
            _ => throw new InvalidProgramException("This is not a valid delegation target")
        };

    private static ITypeSymbol GetParent(SemanticModel model, SyntaxNode member) =>
        member.Parent is not { } parentSyntax ||
        model.GetDeclaredSymbol(parentSyntax) is not ITypeSymbol parent
            ? throw new InvalidProgramException("Cannot find parent of delegated item")
            : parent;

    private static IDelegatedMethodGenerator? ParseFromMethod(SemanticModel model, MethodDeclarationSyntax mds,
        ITypeSymbol parent) =>
        model.GetDeclaredSymbol(mds) is IMethodSymbol symbol && IsValidDelegatingMethod(symbol)
            ? DelegatedMethodGenerator.Create(symbol.ReturnType, $"this.{symbol.Name}()", mds, parent)
            : null;

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;

    private static IDelegatedMethodGenerator? ParseFromProperty(SemanticModel model, PropertyDeclarationSyntax pds,
        ITypeSymbol parent) => 
        model.GetDeclaredSymbol(pds) is IPropertySymbol ps ? 
            DelegatedMethodGenerator.Create(ps.Type, $"this.{ps.Name}", pds, parent) : 
            null;

    private static IDelegatedMethodGenerator? ParseFromField(SemanticModel model, FieldDeclarationSyntax fs,
        ITypeSymbol parent) =>
        model.GetDeclaredSymbol(FirstVariableDecl(fs)) is IFieldSymbol symbol?
            DelegatedMethodGenerator.Create(symbol.Type, $"this.{symbol.Name}", fs, parent):
            null;

    private static VariableDeclaratorSyntax FirstVariableDecl(FieldDeclarationSyntax fs) => 
        fs.Declaration.Variables.First();
}