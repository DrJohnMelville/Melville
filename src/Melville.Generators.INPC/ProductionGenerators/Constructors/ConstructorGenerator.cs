using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.Constructors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

[Generator]
public class ConstructorGenerator : ClassWithLabeledMembersGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.FromConstructorAttribute");

    public ConstructorGenerator() : base(attrFinder)
    {
    }

    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) =>
        new ConstructorClassSyntaxModel(targetTypeDecl);
}