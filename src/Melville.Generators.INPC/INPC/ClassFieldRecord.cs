using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC;

public class ClassFieldRecord{
    public List<FieldDeclarationSyntax> FieldsToWrap { get; } = new();
    public List<PropertyDeclarationSyntax> ProperteisToMap { get; } = new();
    public TypeDeclarationSyntax ClassDeclaration { get; }

    public ClassFieldRecord(TypeDeclarationSyntax classDeclaration)
    {
        ClassDeclaration = classDeclaration;
    }

    public void AddField(FieldDeclarationSyntax field)
    {
        FieldsToWrap.Add(field);
    }
    public void AddProperty(PropertyDeclarationSyntax prop)
    {
        ProperteisToMap.Add(prop);
    }

    public ClassToImplement ElaborateSemanticInfo(SemanticModel semanticModel)
    {
        return new ClassToImplement(FieldsToWrap, ClassDeclaration,
            semanticModel, 
            MapPropertyDependencies());
    }

    private PropertyDependencyChecker MapPropertyDependencies()
    {
        var pc = new PropertyDependencyChecker();
        foreach (var propertyDeclarationSyntax in ProperteisToMap)
        {
            pc.AddProperty(propertyDeclarationSyntax);
        }

        return pc;
    }
}