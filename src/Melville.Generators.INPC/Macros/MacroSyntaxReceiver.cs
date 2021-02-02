using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Macros
{
    public class MacroRequest
    {
        public TypeDeclarationSyntax NodeToEnclose { get; }
        public string GeneratedCode { get; }

        public MacroRequest(TypeDeclarationSyntax nodeToEnclose, string generatedCode)
        {
            this.NodeToEnclose = nodeToEnclose;
            this.GeneratedCode = generatedCode;
        }
    }

    public class MacroSyntaxReceiver: ISyntaxReceiver
    {
        public List<MacroRequest> Requests { get; } = new();
        private SearchForAttribute codeFinder = new("Melville.MacroGen.MacroCodeAttribute");
        private SearchForAttribute itemFinder = new("Melville.MacroGen.MacroItemAttribute");

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is MemberDeclarationSyntax mds)) return;
            var codeAttrs = codeFinder.FindAllAttributes(mds).ToList();
            if (codeAttrs.Count == 0) return;
            var items = itemFinder.FindAllAttributes(mds).ToList();
            if (items.Count == 0) return;
            
            Requests.Add(new MacroRequest(EnclosingType(syntaxNode), 
                GenerateCombinations(codeAttrs, items)
                ));
            
        }

        private string GenerateCombinations(List<AttributeSyntax> codeAttrs, List<AttributeSyntax> items)
        {
            return StringJoin(
                codeAttrs, AllParameters(items));
        }

        private List<List<string>> AllParameters(List<AttributeSyntax> items)
        {
            return items.Select(i=>AttributeParameters(i).ToList()).ToList();
        }

        private string StringJoin(IList<AttributeSyntax> templates, List<List<string>> attrs)
        {
            var ret = new StringBuilder();
            foreach (var template in templates.Select(i=>i.ArgumentList).Where(i=>i!=null))
            {
                var exp = new MacroExpander(template!);
                exp.Expand(ret, attrs);
            }

            return ret.ToString();
        }


        private IEnumerable<string> AttributeParameters(AttributeSyntax attrib) =>
            attrib.ArgumentList?.Arguments.Select(AttributeParsers.ExtractArgumentFromAttribute)
            ?? Array.Empty<string>();


        private TypeDeclarationSyntax EnclosingType(SyntaxNode syntaxNode)
        {
            return syntaxNode is TypeDeclarationSyntax td ? td : EnclosingType(
                syntaxNode.Parent ?? throw new InvalidOperationException("No enclosing type"));
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
    }
}