using System;
using System.Collections.Generic;
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
            writer.Append(classDecl.Modifiers.ToString());
            writer.Append(" ");
            writer.Append(classDecl.Keyword.ToString());
            writer.Append(" ");
            writer.Append(classDecl.Identifier.ToString());
            if (classDecl.TypeParameterList is { } tpl) writer.Append(tpl.ToString());
            writer.Append(suffix);
            writer.Append(" ");
            writer.Append(classDecl.ConstraintClauses.ToString());
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