using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Macros
{
    public static class MacroSyntaxInterpreter
    {
        private static SearchForAttribute codeFinder = new("Melville.MacroGen.MacroCodeAttribute");
        private static SearchForAttribute itemFinder = new("Melville.MacroGen.MacroItemAttribute");

        public static void ExpandSingleMacroSet(SyntaxNode syntaxNode, CodeWriter cw)
        {
            if (!(syntaxNode is MemberDeclarationSyntax mds)) return;
            var codeAttrs = codeFinder.FindAllAttributes(mds).ToList();
            if (codeAttrs.Count == 0) return;
            var items = itemFinder.FindAllAttributes(mds).ToList();
            if (items.Count == 0) return;
            
            ImplementCartesianProduct(
                codeAttrs, AllParameters(items), cw);
        }

        private static List<List<string>> AllParameters(List<AttributeSyntax> items) => 
            items.Select(i=> i.AttributeParameters().ToList()).ToList();

        private static void ImplementCartesianProduct
            (IList<AttributeSyntax> templates, List<List<string>> attrs, CodeWriter codeWriter)
        {
            foreach (var template in templates.Select(i=>i.ArgumentList).Where(i=>i!=null))
            {
                var exp = new MacroExpander(template!);
                exp.Expand(codeWriter, attrs);
            }
        }
    }
}