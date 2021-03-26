using System;
using Melville.Generators.INPC.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.CodeWriters
{
    public static class NameSpaceBlockWriter
    {
        public static IDisposable GeneratePartialClassContext(this CodeWriter outerWriter, SyntaxNode node)
        {
            DuplicateEnclosingUsingDeclarations(outerWriter, node);
            return GenerateEnclosingNamemspaces(outerWriter, node);
        }

        private static IDisposable GenerateEnclosingNamemspaces(CodeWriter outerWriter, SyntaxNode node) => 
            outerWriter.EnclosingBlockWriter<NamespaceDeclarationSyntax>(node, EminNamespaceDeclaratioText);

        private static void EminNamespaceDeclaratioText(CodeWriter writer, NamespaceDeclarationSyntax ns)
        {
            writer.Append("namespace ");
            writer.AppendLine(ns.Name.ToString());
        }

        private static void DuplicateEnclosingUsingDeclarations(CodeWriter outerWriter, SyntaxNode node)
        {
            foreach (var usingDecl in node.UsingDeclarationsInScope())
            {
                outerWriter.AppendLine(usingDecl.ToString());
            }
        }
    }
}