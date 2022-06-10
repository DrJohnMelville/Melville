using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public class GeneratorContextCodeWriter: CodeWriter
{
    private GeneratorExecutionContext context;
    public GeneratorContextCodeWriter(GeneratorExecutionContext context)
    {
        this.context = context;
    }
    public override void PublishCodeInFile(string fileName) =>
        context.AddSource(fileName, SourceText.From(this.ToString(), Encoding.UTF8));
    public override void ReportDiagnostic(Diagnostic diagnostic) => context.ReportDiagnostic(diagnostic);
}