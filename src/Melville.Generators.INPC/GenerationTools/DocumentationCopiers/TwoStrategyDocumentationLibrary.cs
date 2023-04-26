using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public partial class DocumentationFromSymbolOrPath
{
    private class TwoStrategyDocumentationLibrary : IDocumentationLibrary
    {
        private readonly IDocumentationLibrary alternate;
        private readonly string path;

        public TwoStrategyDocumentationLibrary(IDocumentationLibrary alternate, string path)
        {
            this.alternate = alternate;
            this.path = path;
        }

        public string LookupDocumentationFor(ISymbol symbol) =>
            TryStrategy(symbol, AlwaysUseSymbolMethod.Instance) ??
            TryStrategy(symbol, alternate) ??
            "";

        private string? TryStrategy(ISymbol symbol, IDocumentationLibrary strategy)
        {
            if (strategy.LookupDocumentationFor(symbol) is { Length: > 0 } text)
            {
                SetStrategyForPath(path, strategy.OptimizedLibrary());
                return text;
            }

            return null;
        }

        public IDocumentationLibrary OptimizedLibrary() => this;
    }
}