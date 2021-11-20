using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.PartialTypeGenerators;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.Macros;

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