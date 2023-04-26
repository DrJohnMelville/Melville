using System;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public readonly struct DocumentationCopier
{
    private readonly CodeWriter cw;

    public DocumentationCopier(CodeWriter cw)
    {
        this.cw = cw;
    }

    public void Copy(ISymbol symbol, IDocumentationLibrary provider) => 
        Copy(provider.LookupDocumentationFor(symbol));
    public void Copy(ISymbol symbol) => Copy(symbol.GetDocumentationCommentXml());

    public void Copy(string? docComment)
    {
        if (docComment is not {Length:>0}) return;
        var firstIndex = docComment.IndexOf('>');
        var lastIndex = docComment.LastIndexOf('<');
        if (firstIndex < 0 || lastIndex < 0) return;
        firstIndex++;
        if (firstIndex >= lastIndex) return;
        WriteSpanInDocCommentBlock(docComment.AsSpan(firstIndex, lastIndex - firstIndex));
    }

    private void WriteSpanInDocCommentBlock(in ReadOnlySpan<char> docComment)
    {
        cw.CopyWithLinePrefix("/// ", docComment);
    }

}