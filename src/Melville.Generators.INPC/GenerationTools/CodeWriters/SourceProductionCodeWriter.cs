using System;
using System.Text;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public class SourceProductionCodeWriter: CodeWriter
{
    private readonly SourceProductionContext context;

    public SourceProductionCodeWriter(SourceProductionContext context)
    {
        this.context = context;
    }

    public override void PublishCodeInFile(string fileName)
    {
        context.AddSource(fileName, SourceText.From(ToString(), Encoding.UTF8));
    }

    public override void ReportDiagnostic(Diagnostic diagnostic) =>
        context.ReportDiagnostic(diagnostic);

    public SourceProductionCodeOperation GenerateInClassFile(
        ISymbol symbol, string prefix, string baseDeclaration = "") =>
        GenerateInClassFile(symbol.DeclaringSyntaxReferences[0].GetSyntax(), prefix, baseDeclaration);
    public SourceProductionCodeOperation GenerateInClassFile(
        SyntaxNode node, string prefix, string baseDeclaration = "") =>
        new SourceProductionCodeOperation(this, node, prefix, baseDeclaration);
}

public readonly struct SourceProductionCodeOperation : IDisposable
{
    private readonly IDisposable closeCodeBlocks;
    private readonly SourceProductionCodeWriter writer;
    private readonly SyntaxNode node;
    private readonly string prefix;

    public SourceProductionCodeOperation(
        SourceProductionCodeWriter writer, SyntaxNode node, string prefix, string baseDeclaration = "")
    {
        this.writer = writer;
        this.node = node;
        this.prefix = prefix;
        closeCodeBlocks = WriteCodeNear.Symbol(node, writer, baseDeclaration);
    }

    public void Dispose()
    {
        closeCodeBlocks.Dispose();
        writer.PublishCodeInFile(node, prefix);
    }
}