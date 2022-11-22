using System;
using System.Text;
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

    public SourceProductionCodeOperation GenerateInClassFile(SyntaxNode node, string prefix) =>
        new SourceProductionCodeOperation(this, node, prefix);
}

public readonly struct SourceProductionCodeOperation : IDisposable
{
    private readonly IDisposable closeCodeBlocks;
    private readonly SourceProductionCodeWriter writer;
    private readonly SyntaxNode node;
    private readonly string prefix;

    public SourceProductionCodeOperation(SourceProductionCodeWriter writer, SyntaxNode node, string prefix)
    {
        this.writer = writer;
        this.node = node;
        this.prefix = prefix;
        closeCodeBlocks = WriteCodeNear.Symbol(node, writer);
    }

    public void Dispose()
    {
        closeCodeBlocks.Dispose();
        writer.PublishCodeInFile(node, prefix);
    }
}