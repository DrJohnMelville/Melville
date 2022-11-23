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

    public string? InheritFrom() => null;

    public void GenerateForwardingMethods(CodeWriter cw) =>
        cw.ReportDiagnosticAt(location, "Dele001", "Invalid Delegation target",
            $"Do not know how to generate delegating methods for a {target.TypeKind}",
            DiagnosticSeverity.Error);
}