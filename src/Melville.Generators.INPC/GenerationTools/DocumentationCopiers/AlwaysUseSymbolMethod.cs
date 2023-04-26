using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public class AlwaysUseSymbolMethod : IDocumentationLibrary
{
    public static AlwaysUseSymbolMethod Instance = new();
    private AlwaysUseSymbolMethod(){}

    public string LookupDocumentationFor(ISymbol symbol) => symbol.GetDocumentationCommentXml() ?? "";

    public IDocumentationLibrary OptimizedLibrary() => this;

}