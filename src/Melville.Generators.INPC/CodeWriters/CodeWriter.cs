using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.CodeWriters
{
    public class CodeWriter
    {
        public GeneratorExecutionContext Context { get; }
        private readonly HashSet<string> prefixLines = new();
        private readonly IndentedStringBuilder target = new();

        public CodeWriter(GeneratorExecutionContext context)
        {
            Context = context;
        }

        public void PublishCodeInFile(string fileName) =>
            Context.AddSource(fileName, SourceText.From(this.ToString(), Encoding.UTF8));

        public void AddPrefixLine(string line) => prefixLines.Add(line);
        public void AppendLine(string s = "") => target.AppendLine(s);
        public void Append(string s) => target.Append(s);
        public IDisposable CurlyBlock() => target.CurlyBlock();
        public IDisposable IndentedRun() => target.IndentedRun();
        public override string ToString() => string.Join(Environment.NewLine, prefixLines.Append(target.ToString()));

        public void ReportDiagnostic(Diagnostic diagnostic) => Context.ReportDiagnostic(diagnostic);
    }

    public static class CodeWriterOperations
    {
        public static void ReportDiagnosticAt(
            this CodeWriter cw, SyntaxNode location, string key, string title, string error, 
            DiagnosticSeverity severity)
        {
            cw.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(key, title, error, "Generation", severity, true),
                Location.Create(location.SyntaxTree, location.Span)));
        }

        public static void CopyAttributes(this CodeWriter cw, IEnumerable<AttributeListSyntax> attrs,
            string? excludeAttribute = null)
        {
            foreach (var attr in attrs.SelectMany(i=>i.Attributes))
            {
                if (!attr.Name.ToString().Equals(excludeAttribute, StringComparison.Ordinal))
                {
                    cw.AppendLine($"[{attr}]");
                }
            }

        }
    }
}
