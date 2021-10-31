using System;
using System.Linq;
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
            GenerateFileScopedNamespace(outerWriter, node);
            return GenerateEnclosingNamemspaceBlockss(outerWriter, node);
        }

        private static void GenerateFileScopedNamespace(CodeWriter outerWriter, SyntaxNode node)
        {
            if (TryFindFileScopedNamespace(node) is {} fsd)
                OutputFileScopedNamespaceDeclaration(outerWriter, fsd);
        }

        private static FileScopedNamespaceDeclarationSyntax? TryFindFileScopedNamespace(SyntaxNode node) => 
            node.Ancestors()
                .OfType<FileScopedNamespaceDeclarationSyntax>()
                .FirstOrDefault();

        private static void OutputFileScopedNamespaceDeclaration(CodeWriter outerWriter,
            FileScopedNamespaceDeclarationSyntax fsd)
        {
            outerWriter.Append("namespace ");
            outerWriter.Append(fsd.Name.ToString());
            outerWriter.AppendLine(";");
        }

        private static IDisposable GenerateEnclosingNamemspaceBlockss(CodeWriter outerWriter, SyntaxNode node) => 
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