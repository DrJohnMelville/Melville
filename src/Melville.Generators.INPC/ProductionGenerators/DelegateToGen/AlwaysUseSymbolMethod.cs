using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class AlwaysUseSymbolMethod : IDocumentationLibrary
{
    public static AlwaysUseSymbolMethod Instance = new();
    private AlwaysUseSymbolMethod(){}

    public string LookupDocumentationFor(ISymbol symbol) => symbol.GetDocumentationCommentXml() ?? "";

    public IDocumentationLibrary OptimizedLibrary() => this;

}