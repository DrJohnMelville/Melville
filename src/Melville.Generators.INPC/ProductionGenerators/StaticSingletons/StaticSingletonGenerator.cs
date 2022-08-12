using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;

[Generator]
public class StaticSingletonGenerator: ClassWithLabeledMembersGenerator
{
    public static readonly SearchForAttribute AttributeFinder = new("Melville.INPC.StaticSingletonAttribute");
    public StaticSingletonGenerator() : base(AttributeFinder)
    {
    }

    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) => 
        new StaticSingletonLabeledMember(targetTypeDecl);
}