using System;
using Melville.Generators.INPC.INPC;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.Common.CodeWriters
{
    public static class EnclosingClassWriter
    {

        public static IDisposable GenerateEnclosingClasses(this CodeWriter writer, SyntaxNode node,
            string innermostSuffix) =>
            writer.EnclosingBlockWriter<ClassDeclarationSyntax>(node, 
                (cw,cd)=>WriteSingleClassDecl(cw, cd, ""),
                (cw,cd)=>WriteSingleClassDecl(cw,cd, innermostSuffix));

        private static void WriteSingleClassDecl(CodeWriter writer, ClassDeclarationSyntax classDecl,
            string suffix)
        {
            CheckForPartialDeclaration(writer, classDecl);
            CopyClassDeclarationBeforeOpeningBrace(writer, classDecl, suffix);
        }

        private static void CopyClassDeclarationBeforeOpeningBrace(CodeWriter writer, 
            ClassDeclarationSyntax classDecl, string suffix)
        {
            const int CSharpAttributeListKind = 8847;
            foreach (var token in classDecl.ChildNodesAndTokens())
            {
                if (token.RawKind == CSharpAttributeListKind) continue;
                if (token == classDecl.BaseList) continue;
                if (token == classDecl.OpenBraceToken) break;
                writer.Append(token.ToString());
                writer.Append(" ");
            }
            writer.Append(suffix);
            writer.AppendLine();
        }

        private static void CheckForPartialDeclaration(CodeWriter writer, ClassDeclarationSyntax classDeclaration)
        {
            if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                writer.ReportDiagnostic(Diagnostic.Create(
                    NeedPartialDiagnostic(classDeclaration.Identifier.ToString()),
                    Location.Create(classDeclaration.SyntaxTree, classDeclaration.Modifiers.Span)));
            } 
        }
        private static DiagnosticDescriptor NeedPartialDiagnostic(string className)
        {
            return new DiagnosticDescriptor("INPCGen0001", "Classes with generated enhancement must be partial.",
                $"Class '{className}' must be declared partial to auto-generate features",
                "Generation", DiagnosticSeverity.Error, true);
        }
    }
}