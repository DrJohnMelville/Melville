using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public readonly struct AttributeCopier
{
    private readonly CodeWriter cw;
    private readonly string attributePrefix;

    public AttributeCopier(CodeWriter cw, string attributePrefix)
    {
        this.cw = cw;
        this.attributePrefix = attributePrefix;
    }

    public void CopyAttributesFrom(SyntaxNode sym)
    {
        if (sym.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault() is {} mds)
            CopyAttributesFrom(mds);
    }

    public void CopyAttributesFrom(MemberDeclarationSyntax mds)
    {
        CopyAttributesFrom(mds.AttributeLists);
    }

    public void CopyAttributesFrom(IEnumerable<AttributeListSyntax> attrs)
    {
        foreach (var attributeListSyntax in attrs)
        {
            if (!IsDesiredAttributeTarget(attributeListSyntax)) continue;
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                TryCopyAttributeFrom(attributeSyntax);
            }
        }
    }

    private bool IsDesiredAttributeTarget(AttributeListSyntax attributeListSyntax) =>
        attributePrefix.Equals(
            attributeListSyntax.Target?.Identifier.ValueText, StringComparison.Ordinal);

    private void TryCopyAttributeFrom(AttributeSyntax attributeSyntax)
    {
        cw.AppendLine($"[{attributeSyntax}]");
    }
}