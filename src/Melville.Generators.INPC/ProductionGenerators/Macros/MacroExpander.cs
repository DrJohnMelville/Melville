using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

public readonly struct MacroExpander
{
    private readonly ImmutableArray<AttributeData> attributes;
    private readonly List<ImmutableArray<TypedConstant>> replacements;

    public MacroExpander(ImmutableArray<AttributeData> attributes)
    {
        this.attributes = attributes;
        replacements = attributes
            .FilterToAttributeType("Melville.INPC.MacroItemAttribute")
            .Select(i => i.ConstructorArguments[0].Values)
            .ToList();
    }

    public void WriteMacros(CodeWriter cw)
    {
        foreach (var element in CodeElements())
        {
            element.WriteCode(cw, replacements);
        }
    }

    private IEnumerable<MacroCodeExpander> CodeElements()
    {
        foreach (var codeAttribute in attributes.FilterToAttributeType("Melville.INPC.MacroCodeAttribute"))
        {
            var (prefix, postfix) = ReadPrefixAndPostFix(codeAttribute);
            yield return new MacroCodeExpander(prefix, ReadCodeParameter(codeAttribute), postfix);
        }
    }

    private static string ReadCodeParameter(AttributeData codeAttribute) =>
        codeAttribute.ConstructorArguments[0].CodeString();

    private static (string prefix, string postfix) ReadPrefixAndPostFix(AttributeData codeAttribute)
    {
        var prefix = "";
        var postfix = "";
        foreach (var argPair in codeAttribute.NamedArguments)
        {
            switch (argPair.Key)
            {
                case "Prefix":
                    prefix = argPair.Value.CodeString();
                    break;
                case "Postfix":
                    postfix = argPair.Value.CodeString();
                    break;
            }
        }

        return (prefix, postfix);
    }
}