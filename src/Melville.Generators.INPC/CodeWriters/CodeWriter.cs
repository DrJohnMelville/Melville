using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.CodeWriters;

public abstract  class CodeWriter
{
    private readonly HashSet<string> prefixLines = new();
    private readonly IndentedStringBuilder target = new();
    
    public void AddPrefixLine(string line) => prefixLines.Add(line);
    public void AppendLine(string s = "") => target.AppendLine(s);
    public void Append(string s) => target.Append(s);
    public IDisposable CurlyBlock() => target.CurlyBlock();
    public IDisposable IndentedRun() => target.IndentedRun();
    public override string ToString() => string.Join(Environment.NewLine, prefixLines.Append(target.ToString()));
    
    public abstract void PublishCodeInFile(string fileName);
    public abstract void ReportDiagnostic(Diagnostic diagnostic);
}

public class PostInitializationCodeWriter: CodeWriter
{
    private readonly IncrementalGeneratorPostInitializationContext context;

    public PostInitializationCodeWriter(IncrementalGeneratorPostInitializationContext context)
    {
        this.context = context;
    }

    public override void PublishCodeInFile(string fileName) => 
        context.AddSource(fileName, SourceText.From(ToString(), Encoding.UTF8));

    public override void ReportDiagnostic(Diagnostic diagnostic) =>
        throw new NotSupportedException("Cannot generate diagnostics in post initialization");
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