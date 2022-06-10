using System;
using System.Collections.Generic;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC;

public class ClassToImplement
{
    public List<FieldDeclarationSyntax> FieldsToWrap { get; }
    public TypeDeclarationSyntax ClassDeclaration { get; }
    public SemanticModel SemanticModel { get; }
    public PropertyDependencyChecker PropertyDependencies { get; }
    public INamedTypeSymbol TypeInfo { get; }

    public ClassToImplement(List<FieldDeclarationSyntax> fieldsToWrap, TypeDeclarationSyntax classDeclaration,
        SemanticModel semanticModel, PropertyDependencyChecker propertyDependencies)
    {
        FieldsToWrap = fieldsToWrap;
        ClassDeclaration = classDeclaration;
        SemanticModel = semanticModel;
        PropertyDependencies = propertyDependencies;
        TypeInfo = semanticModel.GetDeclaredSymbol(ClassDeclaration) ??
                   throw new InvalidProgramException("Class declaration is not a type declaration");
    }

    public void GenerateCode(SourceProductionContext context)
    {
        var cw = new SourceProductionCodeWriter(context);
        InpcClassGeneratorFactory.CreateGenerator(this, cw).WriteToCodeWriter();
        cw.PublishCodeInFile(ClassDeclaration, "INPC");
    }
}