using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DelegateToGen
{
    public static class DelegationRequestParser
    {
        public static DelegationRequest Parse(
            SemanticModel model, TypeDeclarationSyntax type, IEnumerable<MemberDeclarationSyntax> members)
        {
            if (!(model.GetDeclaredSymbol(type) is INamedTypeSymbol classSymbol))
                throw new InvalidProgramException("Class decl must resolve to a type");
            return new DelegationRequest(classSymbol, ParseItems(model, members));
        }

        private static List<IDelegatedMethodGenerator> ParseItems(
            SemanticModel model, IEnumerable<MemberDeclarationSyntax> members) =>
            members.Select(i=>ParseItem(model, i)).OfType<IDelegatedMethodGenerator>().ToList();

        private static IDelegatedMethodGenerator? ParseItem(
            SemanticModel model, MemberDeclarationSyntax member) =>
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
                ? DelegatedMethodGenerator.Create(symbol.ReturnType, $"this.{symbol.Name}().", mds)
                : null;

        private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
            symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;

        private static IDelegatedMethodGenerator? ParseFromProperty(
            SemanticModel model, PropertyDeclarationSyntax pds) => 
            model.GetDeclaredSymbol(pds) is IPropertySymbol ps ? 
                DelegatedMethodGenerator.Create(ps.Type, $"this.{ps.Name}.", pds) : 
                null;

        private static IDelegatedMethodGenerator? ParseFromField(
            SemanticModel model, FieldDeclarationSyntax fs) =>
            model.GetDeclaredSymbol(FirstVariableDecl(fs)) is IFieldSymbol symbol?
                DelegatedMethodGenerator.Create(symbol.Type, $"this.{symbol.Name}.", fs):
                null;

        private static VariableDeclaratorSyntax FirstVariableDecl(FieldDeclarationSyntax fs) => 
            fs.Declaration.Variables.First();
    }
}