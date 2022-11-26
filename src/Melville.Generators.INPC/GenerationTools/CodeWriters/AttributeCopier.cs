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
    private readonly AttributeWriterStrategy writerStrategy;

    public AttributeCopier(CodeWriter cw, string attributePrefix) : this(cw, attributePrefix,
        AttributeWriterStrategy.OnePerLine)
    {
    }

    public AttributeCopier(CodeWriter cw, string attributePrefix, AttributeWriterStrategy writerStrategy)
    {
        this.cw = cw;
        this.attributePrefix = attributePrefix;
        this.writerStrategy = writerStrategy;
    }

    public void CopyAttributesFrom(IEnumerable<SyntaxReference> referemces)
    {
        foreach (var reference in referemces)
        {
            CopyAttributesFrom(reference.GetSyntax());
        }
    }

    public void CopyAttributesFrom(SyntaxNode? sym)
    {
        switch (sym)
        {
            case null: return;
            case MemberDeclarationSyntax mds: CopyAttributesFrom(mds.AttributeLists); break;
            case ParameterSyntax ps: CopyAttributesFrom(ps.AttributeLists); break;
            default: CopyAttributesFrom(sym.Parent); break;
        }
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
                writerStrategy.RenderAttribute(cw, attributeSyntax);
            }
        }
    }

    private bool IsDesiredAttributeTarget(AttributeListSyntax attributeListSyntax) =>
        attributePrefix.Length == 0 ||
        attributePrefix.Equals(
            attributeListSyntax.Target?.Identifier.ValueText, StringComparison.Ordinal);
}