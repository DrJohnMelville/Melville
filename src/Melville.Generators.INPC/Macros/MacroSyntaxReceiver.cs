using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Macros
{
    public static class MacroSyntaxReceiver
    {
        private static SearchForAttribute codeFinder = new("Melville.MacroGen.MacroCodeAttribute");
        private static SearchForAttribute itemFinder = new("Melville.MacroGen.MacroItemAttribute");

        public static void OnVisitSyntaxNode(SyntaxNode syntaxNode, CodeWriter cw)
        {
            if (!(syntaxNode is MemberDeclarationSyntax mds)) return;
            var codeAttrs = codeFinder.FindAllAttributes(mds).ToList();
            if (codeAttrs.Count == 0) return;
            var items = itemFinder.FindAllAttributes(mds).ToList();
            if (items.Count == 0) return;
            
            cw.AppendLine(GenerateCombinations(codeAttrs, items));
        }

        private static string GenerateCombinations(List<AttributeSyntax> codeAttrs, List<AttributeSyntax> items)
        {
            return ImplementCartesianProduct(
                codeAttrs, AllParameters(items));
        }

        private static List<List<string>> AllParameters(List<AttributeSyntax> items)
        {
            return items.Select(i=> i.AttributeParameters().ToList()).ToList();
        }

        private static string ImplementCartesianProduct
            (IList<AttributeSyntax> templates, List<List<string>> attrs)
        {
            var ret = new StringBuilder();
            foreach (var template in templates.Select(i=>i.ArgumentList).Where(i=>i!=null))
            {
                var exp = new MacroExpander(template!);
                exp.Expand(ret, attrs);
            }

            return ret.ToString();
        }
    }

    public ref struct MacroExpander
    {
        private readonly string? prefix;
        private readonly string? postfix;
        private readonly string text;

        public MacroExpander(AttributeArgumentListSyntax attrs)
        {
            prefix = postfix = null;
            text = "";
            foreach (var attr in attrs.Arguments)
            {
                switch (attr.NameEquals?.Name.ToString(), attr.ExtractArgumentFromAttribute())
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

        public void Expand(StringBuilder sb, List<List<string>> values)
        {
            if (!string.IsNullOrWhiteSpace(prefix)) sb.AppendLine(prefix);
            foreach (var value in values)
            {
                sb.AppendLine(Replace(text, value));
            }
            if (!string.IsNullOrWhiteSpace(postfix)) sb.AppendLine(postfix);
        }
        
        private static Regex RegexReplacer = new Regex(@"~(\d+)~");
        private string Replace(string template, IList<string> attrList)
        {
            return RegexReplacer.Replace(template, m => attrList[int.Parse(m.Groups[1].Value)]);
        }
    }

    public static class AttributeParsers
    {
        public static string ExtractArgumentFromAttribute(this AttributeArgumentSyntax arg)
        {
            return arg.Expression switch
            {
                LiteralExpressionSyntax les when les.Kind() ==SyntaxKind.StringLiteralExpression 
                    => les.Token.ValueText,
                _ => arg.ToString()
            };
        }

        public static IEnumerable<string> AttributeParameters(this AttributeSyntax attrib) =>
            attrib.ArgumentList?.Arguments.Select(AttributeParsers.ExtractArgumentFromAttribute)
            ?? Array.Empty<string>();
    }
}