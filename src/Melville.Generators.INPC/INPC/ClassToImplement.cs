using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public class ClassToImplement
    {
        public List<FieldDeclarationSyntax> FieldsToWrap { get; }
        public ClassDeclarationSyntax ClassDeclaration { get; }
        public SemanticModel SemanticModel { get; }
        public PropertyDependencyChecker PropertyDependencies { get; }
        public INamedTypeSymbol TypeInfo { get; }

        public ClassToImplement(List<FieldDeclarationSyntax> fieldsToWrap, ClassDeclarationSyntax classDeclaration,
            SemanticModel semanticModel, PropertyDependencyChecker propertyDependencies)
        {
            FieldsToWrap = fieldsToWrap;
            ClassDeclaration = classDeclaration;
            SemanticModel = semanticModel;
            PropertyDependencies = propertyDependencies;
            TypeInfo = semanticModel.GetDeclaredSymbol(ClassDeclaration) ??
                       throw new InvalidProgramException("Class declaration is not a type declaration");
        }
    }
}