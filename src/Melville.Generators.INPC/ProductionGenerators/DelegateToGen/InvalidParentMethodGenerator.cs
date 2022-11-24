using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class InvalidParentMethodGenerator : IDelegatedMethodGenerator
{
    private readonly ITypeSymbol target;
    private readonly SyntaxNode location;

    public InvalidParentMethodGenerator(ITypeSymbol target, SyntaxNode location)
    {
        this.target = target;
        this.location = location;
    }

    public void GenerateForwardingMethods(CodeWriter cw) =>
        cw.ReportDiagnosticAt(location, "Dele001", "Invalid Delegation target",
            $"Do not know how to generate delegating methods for a {target.TypeKind}",
            DiagnosticSeverity.Error);
}

public class InvalidTargetMethodGenerator : IDelegatedMethodGenerator
{
    private readonly SyntaxNode location;

    public InvalidTargetMethodGenerator(SyntaxNode location)
    {
        this.location = location;
    }

    public void GenerateForwardingMethods(CodeWriter cw) =>
        cw.ReportDiagnosticAt(location, "Dele002", "Invalid Delegation method",
            $"Can only delegate to a non-void returning method with no parameters",
            DiagnosticSeverity.Error);
}