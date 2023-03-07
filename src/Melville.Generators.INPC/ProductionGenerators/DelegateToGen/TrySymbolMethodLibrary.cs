using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class TrySymbolMethodLibrary : IDocumentationLibrary
{
    private readonly IDocumentationLibrary inner;

    public TrySymbolMethodLibrary(IDocumentationLibrary inner)
    {
        this.inner = inner;
    }

    public string LookupDocumentationFor(ISymbol symbol) =>
        symbol.GetDocumentationCommentXml() is { Length: > 0 } native ? 
            native : 
            inner.LookupDocumentationFor(symbol);
}