using System.Linq;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.ProductionGenerators.Constructors;
using Melville.Generators.INPC.ProductionGenerators.INPC;
using Melville.INPC;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32.SafeHandles;

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

public record struct MemberData(string Type, string Name, string LowerCaseName)
{
    public MemberData(string type, string name) : this(type, name, name.AsInstanceSymbol())
    {
    }
}