using System.Collections.Generic;
using Melville.Generators.INPC.INPC.CodeGen;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC;

public class InpcSyntaxModel: ILabeledMembersSyntaxModel{
    private List<FieldDeclarationSyntax> FieldsToWrap { get; } = new();
    private List<PropertyDeclarationSyntax> ProperteisToMap { get; } = new();
    private TypeDeclarationSyntax ClassDeclaration { get; }

    public InpcSyntaxModel(TypeDeclarationSyntax classDeclaration)
    {
        ClassDeclaration = classDeclaration;
    }

    private void AddField(FieldDeclarationSyntax field) => FieldsToWrap.Add(field);

    private void AddProperty(PropertyDeclarationSyntax prop) => ProperteisToMap.Add(prop);

    public void AddMember(SyntaxNode declSyntax)
    {
        switch (declSyntax)
        {
            case FieldDeclarationSyntax fds:
                AddField(fds);
                break;
            case PropertyDeclarationSyntax pds:
                AddProperty(pds);
                break;
        }
    }

    public ILabeledMembersSemanticModel AddSemanticInfo(SemanticModel semanticModel)
    {
        return new InpcSemanticModel(FieldsToWrap, ClassDeclaration,
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