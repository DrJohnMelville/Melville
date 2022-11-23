using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

[Generator]
public class DelegateToGenerator: LabeledMemberGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.DelegateToAttribute");

    public DelegateToGenerator() : base(attrFinder, "GeneratedDelegator")
    {
    }

    protected override bool GenerateCodeForMember(GeneratorSyntaxContext member, CodeWriter cw)
    {
        if (DelegationRequestParser.ParseItem(member.SemanticModel, member.Node) is not { } methodGenerator)
            return false;
        methodGenerator.GenerateForwardingMethods(cw);
        return true;
    }
}