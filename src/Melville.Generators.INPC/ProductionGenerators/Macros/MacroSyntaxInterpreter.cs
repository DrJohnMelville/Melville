using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.Macros;

public static class MacroSyntaxInterpreter
{
    private static SearchForAttribute codeFinder = new("Melville.INPC.MacroCodeAttribute");
    private static SearchForAttribute itemFinder = new("Melville.INPC.MacroItemAttribute");

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
            new MacroExpander(template!).Expand(codeWriter, attrs);
        }
    }
}