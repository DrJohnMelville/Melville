using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.CodeWriters;

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