using System.Collections.Generic;
using Melville.Generators.INPC.Common.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public class INPCReceiver : ISyntaxReceiver
    {
        public Dictionary<ClassDeclarationSyntax, ClassFieldRecord> ClassesToAugment { get; } =
            new Dictionary<ClassDeclarationSyntax, ClassFieldRecord>();
        private SearchForAttribute autoNotifyAttributeSearcher = 
            new SearchForAttribute("Melville.INPC.AutoNotifyAttribute");

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is FieldDeclarationSyntax field &&
                field.Parent is ClassDeclarationSyntax cds &&
                autoNotifyAttributeSearcher.HasAttribute(field))
            {
                GetOrCreateImplementer(cds).AddField(field);
            }else if (syntaxNode is PropertyDeclarationSyntax prop && 
                      prop.Parent is ClassDeclarationSyntax cds2 &&
                      autoNotifyAttributeSearcher.HasAttribute(prop))
            {
                GetOrCreateImplementer(cds2).AddProperty(prop);
            }
        }

        private ClassFieldRecord GetOrCreateImplementer(ClassDeclarationSyntax cds)
        {
            if (ClassesToAugment.TryGetValue(cds, out var cni)) return cni;
            var ret = new ClassFieldRecord(cds);
            ClassesToAugment.Add(cds, ret);
            return ret;
        }
    }
}