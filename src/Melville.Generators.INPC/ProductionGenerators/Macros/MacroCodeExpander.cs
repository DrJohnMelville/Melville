using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

public readonly record struct MacroCodeExpander(string Prefix, string Code, string PostFix)
{
    public void WriteCode(CodeWriter cw, List<ImmutableArray<TypedConstant>> replacements)
    {
        TryWriteNontrivialString(cw, Prefix);
        foreach (var replacement in replacements)
        {
            cw.AppendLine(ReplacedString(replacement));
        }
        TryWriteNontrivialString(cw, PostFix);
    }

    private void TryWriteNontrivialString(CodeWriter cw, string item)
    {
        if (!string.IsNullOrWhiteSpace(item)) cw.AppendLine(item);
    }

    private string ReplacedString(ImmutableArray<TypedConstant> replacements) =>
        RegexReplacer.Replace(Code, m =>
            replacements[int.Parse(m.Groups[1].Value)].CodeString());

    private static Regex RegexReplacer = new Regex(@"~(\d+)~", RegexOptions.Compiled);

}