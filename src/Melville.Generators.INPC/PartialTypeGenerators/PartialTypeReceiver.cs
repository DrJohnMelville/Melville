using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.PartialTypeGenerators
{
    public class PartialTypeReceiver : ISyntaxReceiver
    {
        public List<MemberDeclarationSyntax> TaggedItems = new(); 
        private List<SearchForAttribute> attributeFinders;
        public PartialTypeReceiver(string[] targetAttributes)
        {
            attributeFinders = targetAttributes.Select(i => new SearchForAttribute(i)).ToList();
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is MemberDeclarationSyntax mds &&
                attributeFinders.Any(i => i.HasAttribute(mds)))
            {
                TaggedItems.Add(mds);
            }
        }

        public IEnumerable<IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax>> ItemsByType() => 
            TaggedItems.GroupBy(EnclosingType); 
        
        private TypeDeclarationSyntax EnclosingType(SyntaxNode syntaxNode) =>
            syntaxNode is TypeDeclarationSyntax td ? td : 
                EnclosingType(syntaxNode.Parent ?? throw new InvalidOperationException("No enclosing type"));
    }
}