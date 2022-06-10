using System;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public static class WriteCodeNear
{
    public static IDisposable Symbol(SyntaxNode node, CodeWriter cw, string baseDeclaration = "")
    {
        var namespaces = cw.GeneratePartialClassContext(node);
        var classes = cw.GenerateEnclosingClasses(node, baseDeclaration);
        return new CompositeDispose(new[] { namespaces, classes });
    }
}