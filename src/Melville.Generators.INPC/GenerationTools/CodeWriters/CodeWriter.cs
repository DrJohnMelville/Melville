using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public abstract class CodeWriter
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
        var hashString = string.Join("", member
            .AncestorsAndSelf()
            .Select(NameForNode));
        var hash = Fnv.FromString(hashString);
        return $"{ClassName(member)}.{hash:X}.g.cs";
    }

    private static string ClassName(SyntaxNode? node) => node switch
    {
        null => "NoName",
        TypeDeclarationSyntax tds => tds.Identifier.ToString(),
        _ => ClassName(node.Parent)

    };

    private static string NameForNode(SyntaxNode symbol) =>
        NameForNode(symbol, "");
    private static string NameForNode(SyntaxNode symbol, string prefix) => symbol switch
    {
        MethodDeclarationSyntax mds => Concat(mds.Identifier, mds.TypeParameterList, mds.ParameterList),
        PropertyDeclarationSyntax pds => pds.Identifier.ToString(),
        EventDeclarationSyntax eds => eds.Identifier.ToString(),
        IndexerDeclarationSyntax ids => Concat("Indexer", ids.ParameterList),
        FieldDeclarationSyntax fds => string.Join(
            "", fds.Declaration.Variables.Select(i => i.Identifier.ToString())),
        TypeDeclarationSyntax tds => Concat(tds.Identifier, tds.TypeParameterList),
        BaseTypeDeclarationSyntax btds => btds.Identifier.ToString(),
        BaseNamespaceDeclarationSyntax nds => nds.Name.ToString(),
        CompilationUnitSyntax => prefix,
        _ => ""
    };

    private static string Concat(object identifier, object? trailer1 = null, object? trailer2 = null) => 
        string.Concat(identifier.ToString(), trailer1?.ToString() ?? "", trailer2?.ToString() ?? "");
}