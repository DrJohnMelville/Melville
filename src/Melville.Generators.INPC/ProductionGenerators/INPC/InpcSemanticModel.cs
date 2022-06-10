using System;
using System.Collections.Generic;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.INPC.CodeGen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;


public class InpcSemanticModel : ILabeledMembersSemanticModel
{
    public List<FieldDeclarationSyntax> FieldsToWrap { get; }
    public TypeDeclarationSyntax ClassDeclaration { get; }
    public SemanticModel SemanticModel { get; }
    public PropertyDependencyChecker PropertyDependencies { get; }
    public INamedTypeSymbol TypeInfo { get; }

    public InpcSemanticModel(List<FieldDeclarationSyntax> fieldsToWrap, TypeDeclarationSyntax classDeclaration,
        SemanticModel semanticModel, PropertyDependencyChecker propertyDependencies)
    {
        FieldsToWrap = fieldsToWrap;
        ClassDeclaration = classDeclaration;
        SemanticModel = semanticModel;
        PropertyDependencies = propertyDependencies;
        TypeInfo = semanticModel.GetDeclaredSymbol(ClassDeclaration) ??
                   throw new InvalidProgramException("Class declaration is not a type declaration");
    }
    
    public void GenerateCode(CodeWriter cw)
    {
        InpcClassGeneratorFactory.CreateGenerator(this, cw).WriteToCodeWriter();
    }
}