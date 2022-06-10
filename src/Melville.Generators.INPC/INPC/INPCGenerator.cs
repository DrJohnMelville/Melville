using System;
using System.Collections.Immutable;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC;

[Generator]
public class INPCGenerator : ClassWithLabeledMembersGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    public INPCGenerator() : base(attrFinder)
    {
    }
    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl)
    {
        return new InpcSyntaxModel(targetTypeDecl);
    }
}