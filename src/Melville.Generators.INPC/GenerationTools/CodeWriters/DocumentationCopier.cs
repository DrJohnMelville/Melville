using System;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public readonly struct DocumentationCopier
{
    private readonly CodeWriter cw;

    public DocumentationCopier(CodeWriter cw)
    {
        this.cw = cw;
    }

    public void Copy(ISymbol symbol) => Copy(symbol.GetDocumentationCommentXml());

    private void Copy(string? docComment)
    {
        if (docComment is not {Length:>0}) return;
        var firstIndex = docComment.IndexOf('>');
        var lastIndex = docComment.LastIndexOf('<');
        if (firstIndex < 0 || lastIndex < 0) return;
        firstIndex++;
        if (firstIndex >= lastIndex) return;
        Copy(docComment.AsSpan(firstIndex, lastIndex - firstIndex));
    }

    private void Copy(in ReadOnlySpan<char> docComment)
    {
        var position = 0;
        while (position < docComment.Length)
        {
            if (char.IsWhiteSpace(docComment[position]))
            {
                position++;
            }
            else
            {
                cw.Append("/// ");
                while (position < docComment.Length && docComment[position] is not '\r' or '\n')
                {
                    cw.Append(docComment[position++]);
                }
                cw.AppendLine();
            }
        }
    }

}