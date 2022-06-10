using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

[Generator]
public class INPCGenerator : ClassWithLabeledMembersGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    public INPCGenerator() : base(attrFinder)
    {
    }
    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) => 
        new InpcSyntaxModel(targetTypeDecl);
}