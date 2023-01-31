using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class ErrorMethodGenerator : IDelegatedMethodGenerator
{
    private readonly SyntaxNode location;
    private readonly string errorNumber;
    private readonly string shortTitle;
    private readonly string explanation;

    public ErrorMethodGenerator(SyntaxNode location, string errorNumber, string shortTitle, string explanation)
    {
        this.location = location;
        this.errorNumber = errorNumber;
        this.shortTitle = shortTitle;
        this.explanation = explanation;
    }

    public void GenerateForwardingMethods(CodeWriter cw) =>
        cw.ReportDiagnosticAt(location, errorNumber, shortTitle, explanation, DiagnosticSeverity.Error);
}