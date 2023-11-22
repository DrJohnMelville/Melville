using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public readonly struct SearchForAttribute
{
    private readonly string qualifiedAttributeName;

    public SearchForAttribute(string qualifiedAttributeName)
    {
        this.qualifiedAttributeName = qualifiedAttributeName;
    }

    public bool HasAttribute(MemberDeclarationSyntax node) => null != FindAttribute(node);

    public AttributeSyntax? FindAttribute(MemberDeclarationSyntax node) =>FindAllAttributes(node).FirstOrDefault();

    public IEnumerable<AttributeSyntax> FindAllAttributes(MemberDeclarationSyntax node)
    {
        return FindSyntaxAttributes(node).Where(IsRequestedAttribute);
    }

    private static IEnumerable<AttributeSyntax> FindSyntaxAttributes(MemberDeclarationSyntax node) =>
        node.AttributeLists.SelectMany(i => i.Attributes);

    private bool IsRequestedAttribute(AttributeSyntax arg) => 
        CheckNameInContext(ExpandExplicitAttribute(arg.Name.ToString()), arg);

    private static string ExpandExplicitAttribute(string name) =>
        name.EndsWith("Attribute") ? name : name + "Attribute";

    private bool CheckNameInContext(string attributeName, SyntaxNode context)
    {
        if (qualifiedAttributeName.Equals(attributeName)) return true;
        if (!qualifiedAttributeName.EndsWith(attributeName)) return false;
        return SearchParentNameSpaces(attributeName, context);
    }

    private bool SearchParentNameSpaces(string attributeName, SyntaxNode context)
    {
        var oldList = new List<string> {attributeName};
        foreach (var usingList in context.UsingDeclarationListsForSurroundingScopes())
        {
            IList<string> newList = usingList.Select(i => i.Name?.ToString()??"")
                .SelectMany(ns => oldList,
                    (ns, item) => string.Concat(ns, ".", item))
                .ToList();
            foreach (var item in newList)
            {
                if (qualifiedAttributeName.Equals(item)) return true;
                oldList.Add(item);
            }
        }

        return false;
    }
}