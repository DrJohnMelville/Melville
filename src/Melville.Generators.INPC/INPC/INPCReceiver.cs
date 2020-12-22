using System.Collections.Generic;
using Melville.Generators.Tools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public class INPCReceiver : ISyntaxReceiver
    {
        public Dictionary<ClassDeclarationSyntax, ClassFieldRecord> ClassesToAugment { get; } = new();
        private SearchForAttribute autoNotifyAttributeSearcher = new("Melville.INPC.AutoNotifyAttribute");

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            switch (syntaxNode, syntaxNode.Parent)
            {
                case (FieldDeclarationSyntax field, ClassDeclarationSyntax cds)
                    when autoNotifyAttributeSearcher.HasAttribute(field):
                    GetOrCreateImplementer(cds).AddField(field);
                    break;
                case (PropertyDeclarationSyntax prop, ClassDeclarationSyntax cds)
                    when autoNotifyAttributeSearcher.HasAttribute(prop):
                    GetOrCreateImplementer(cds).AddProperty(prop);
                    break;
                case (ClassDeclarationSyntax cds, _) 
                    when autoNotifyAttributeSearcher.HasAttribute(cds):
                    GetOrCreateImplementer(cds);
                    break;
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