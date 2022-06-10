using System.Collections.Generic;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

public readonly ref struct MacroExpander
{
    private readonly string? prefix;
    private readonly string? postfix;
    private readonly string text;

    public MacroExpander(AttributeArgumentListSyntax attrs)
    {
        prefix = postfix = null;
        text = "";
        foreach (var argument in attrs.ToArguments())
        {
            switch (argument.Name, argument.Value)
            {
                case ("Prefix", var val):
                    prefix = val;
                    break;
                case ("Postfix", var val):
                    postfix = val;
                    break;
                case var (name, val) :
                    text = val;
                    break;
            }
        }
    }

    public void Expand(CodeWriter sb, List<List<string>> values)
    {
        if (prefix != null && prefix.Length > 0) sb.AppendLine(prefix);
        foreach (var value in values)
        {
            sb.AppendLine(Replace(text, value));
        }
        if (postfix != null && postfix.Length > 0) sb.AppendLine(postfix);
    }
        
    private static Regex RegexReplacer = new Regex(@"~(\d+)~");
    private string Replace(string template, IList<string> attrList)
    {
        return RegexReplacer.Replace(template, m => attrList[int.Parse(m.Groups[1].Value)]);
    }
}