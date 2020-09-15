using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Common.AstUtilities
{
 
        public class SearchForAttribute
    {
        private string qualifiedAttributeName;

        public SearchForAttribute(string qualifiedAttributeName)
        {
            this.qualifiedAttributeName = qualifiedAttributeName;
        }

        public bool HasAttribute(SyntaxNode node) => null != FindAttribute(node);

        public AttributeSyntax? FindAttribute(SyntaxNode node) =>
            FindSymbbolAttributes(node).FirstOrDefault(IsRequestedAttribute);

        private static IEnumerable<AttributeSyntax> FindSymbbolAttributes(SyntaxNode node) =>
            node switch
            {
                MemberDeclarationSyntax mds => mds.AttributeLists.SelectMany(i => i.Attributes),
                _ => Array.Empty<AttributeSyntax>()
            };

        private bool IsRequestedAttribute(AttributeSyntax arg)
        {
            return CheckNameInContext(ExpandExplicitAttribute(arg.Name.ToString()), arg);
        }

        private static string ExpandExplicitAttribute(string name) => name.EndsWith("Attribute")?name:name+"Attribute";

        private bool CheckNameInContext(string attributeName, SyntaxNode context)
        {
            if (qualifiedAttributeName.Equals(attributeName)) return true;
            if (!qualifiedAttributeName.EndsWith(attributeName)) return false;
            return SearchParentNameSpaces(attributeName, context);
        }

        private bool SearchParentNameSpaces(string attributeName, SyntaxNode context)
        {
            var oldList = new List<string> {attributeName};
            foreach (var usingList in UsingDeclarationListsForSurroundingScopes(context))
            {
                IList<string> newList = usingList.Select(i => i.Name.ToString())
                    .SelectMany(ns => oldList, (ns, item) => string.Concat(ns, ".", item))
                    .ToList();
                foreach (var item in newList)
                {
                    if (qualifiedAttributeName.Equals(item)) return true;
                    oldList.Add(item);
                }
            }

            return false;
        }


        private IEnumerable<SyntaxList<UsingDirectiveSyntax>> UsingDeclarationListsForSurroundingScopes(SyntaxNode context)
        {
            var currentNode = context.Parent;
            while (currentNode != null)
            {
                switch (currentNode)
                {
                    case NamespaceDeclarationSyntax nds:
                        yield return nds.Usings;
                        break;
                    case CompilationUnitSyntax cus:
                        yield return cus.Usings;
                        break;
                }
                currentNode = currentNode.Parent;
            }
        }
    }
}