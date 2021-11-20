using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

    public static void PublishCodeInFile(this CodeWriter cw, SyntaxNode namedAfter, string prefix)
    {
        cw.PublishCodeInFile(FileNameForMember(namedAfter, prefix));
    }
    
    private static string FileNameForMember(SyntaxNode member, string postfix)
    {
        var sb = new StringBuilder();
        foreach (var symbol in member.AncestorsAndSelf().Reverse())
        {
            sb.Append(NameForNode(symbol, postfix));
            sb.Append('.');
        }
        sb.Append("cs");
        return sb.ToString();
    }

    private static string NameForNode(SyntaxNode symbol, string prefix) => symbol switch
    {
        MethodDeclarationSyntax mds => mds.Identifier.ToString(),
        PropertyDeclarationSyntax pds => pds.Identifier.ToString(),
        EventDeclarationSyntax pds => pds.Identifier.ToString(),
        IndexerDeclarationSyntax pds => "Indexer",
        FieldDeclarationSyntax fds => string.Join(
            "", fds.Declaration.Variables.Select(i => i.Identifier.ToString())),
        BaseTypeDeclarationSyntax btds => btds.Identifier.ToString(),
        BaseNamespaceDeclarationSyntax nds => nds.Name.ToString(),
        CompilationUnitSyntax => prefix,
        _ => "Unnamed"
    };

}