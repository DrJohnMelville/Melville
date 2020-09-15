using System;
using Melville.Generators.INPC.Common.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public static class NameSpaceBlockWriter
    {
        public static IDisposable GenerateEnclosingNamespaces(this CodeWriter outerWriter, SyntaxNode node) =>
            outerWriter.EnclosingBlockWriter<NamespaceDeclarationSyntax>(node, (writer,ns) =>
            {
                writer.Append("namespace ");
                writer.AppendLine(ns.Name.ToString());
            });
    }
}