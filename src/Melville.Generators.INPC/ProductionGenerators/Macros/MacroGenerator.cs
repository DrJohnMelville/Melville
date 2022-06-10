using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

[Generator]
public class MacroGenerator : LabeledMemberGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.MacroCodeAttribute");

    public MacroGenerator() : base(attrFinder, "MacroGen")
    {
    }

    protected override bool GenerateCodeForMember(GeneratorSyntaxContext member, CodeWriter cw)
    {
        MacroSyntaxInterpreter.ExpandSingleMacroSet(member.Node, cw);
        return true;

    }
}