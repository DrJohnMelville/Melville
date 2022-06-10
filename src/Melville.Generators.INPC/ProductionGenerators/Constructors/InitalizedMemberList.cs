using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

public readonly struct InitalizedMemberList
{
    public IList<MemberData> FieldsAndProperties { get; } = new List<MemberData>();
    public IList<MemberData[]> Constructors { get; } = new List<MemberData[]>();

    public InitalizedMemberList()
    {
    }

    public void AddMember(SyntaxNode declSyntax)
    {
        switch (declSyntax)
        {
            case PropertyDeclarationSyntax pds:
                AddProperty(pds);
                break;
            case FieldDeclarationSyntax fds:
                AddFields(fds);
                break;
        }
    }

    private void AddProperty(PropertyDeclarationSyntax pds)
    {
        FieldsAndProperties.Add(new(pds.Type.ToString(), pds.Identifier.ToString()));
    }

    private void AddFields(FieldDeclarationSyntax fds)
    {
        foreach (var variable in fds.Declaration.Variables)
        {
            FieldsAndProperties.Add(new(fds.Declaration.Type.ToString(), variable.Identifier.ToString()));
        }
    }

    public void FindParentConstructors(INamedTypeSymbol? target)
    {
        if (target?.BaseType is not {} baseType) return;
        foreach (var constructor in baseType.InstanceConstructors)
        {
            Constructors.Add(constructor.Parameters.Select(CreateConstructorParameter).ToArray());
        }
    }

    private MemberData CreateConstructorParameter(IParameterSymbol arg) => 
        new(arg.Type.FullyQualifiedName(), arg.Name);
}