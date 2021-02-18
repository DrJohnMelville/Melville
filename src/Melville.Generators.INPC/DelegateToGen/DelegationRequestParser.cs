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

        private static List<DelegationRequestMember> ParseItems(
            SemanticModel model, IEnumerable<MemberDeclarationSyntax> members) =>
            members.Select(i=>ParseItem(model, i)).OfType<DelegationRequestMember>().ToList();

        private static DelegationRequestMember? ParseItem(
            SemanticModel model, MemberDeclarationSyntax member) =>
            member switch
            {
                FieldDeclarationSyntax fs => ParseFromField(model, fs),
                PropertyDeclarationSyntax pds => ParseFromProperty(model, pds),
                MethodDeclarationSyntax mds => ParseFromMethod(model, mds),
                _ => throw new InvalidProgramException("This is not a valid delegation target")
            };

        private static DelegationRequestMember? ParseFromMethod(
            SemanticModel model, MethodDeclarationSyntax mds) =>
            model.GetDeclaredSymbol(mds) is IMethodSymbol symbol && IsValidDelegatingMethod(symbol)
                ? new DelegationRequestMember(symbol.ReturnType, $"this.{symbol.Name}().")
                : null;

        private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
            symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;

        private static DelegationRequestMember? ParseFromProperty(
            SemanticModel model, PropertyDeclarationSyntax pds) => 
            model.GetDeclaredSymbol(pds) is IPropertySymbol ps ? 
                new DelegationRequestMember(ps.Type, $"this.{ps.Name}.") : 
                null;

        private static DelegationRequestMember? ParseFromField(
            SemanticModel model, FieldDeclarationSyntax fs) =>
            model.GetDeclaredSymbol(FirstVariableDecl(fs)) is IFieldSymbol symbol?
                new DelegationRequestMember(symbol.Type, $"this.{symbol.Name}."):
                null;

        private static VariableDeclaratorSyntax FirstVariableDecl(FieldDeclarationSyntax fs) => 
            fs.Declaration.Variables.First();
    }
}