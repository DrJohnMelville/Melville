﻿using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.CodeWriters
{
    public static class EnclosingClassWriter
    {

        public static IDisposable GenerateEnclosingClasses(this CodeWriter writer, SyntaxNode node,
            string innermostSuffix) =>
            writer.EnclosingBlockWriter<TypeDeclarationSyntax>(node, 
                (cw,cd)=>WriteSingleClassDecl(cw, cd, ""),
                (cw,cd)=>WriteSingleClassDecl(cw,cd, innermostSuffix));

        private static void WriteSingleClassDecl(CodeWriter writer, TypeDeclarationSyntax classDecl,
            string suffix)
        {
            CheckForPartialDeclaration(writer, classDecl);
            CopyClassDeclarationBeforeOpeningBrace(writer, classDecl, suffix);
        }

        private static void CopyClassDeclarationBeforeOpeningBrace(CodeWriter writer, 
            TypeDeclarationSyntax classDecl, string suffix)
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

        private static void CheckForPartialDeclaration(CodeWriter writer, TypeDeclarationSyntax classDeclaration)
        {
            if (!classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                writer.ReportDiagnosticAt(classDeclaration, "INPCGen0001", "Classes with generated enhancement must be partial.",
                    $"Class '{classDeclaration.Identifier.ToString()}' must be declared partial to auto-generate features",
                    DiagnosticSeverity.Error);
            } 
        }
    }
}