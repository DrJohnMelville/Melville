using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public partial class DocumentationFromSymbolOrPath : IDocumentationLibrary
{
    private Compilation compilation;
    private static readonly Dictionary<string, IDocumentationLibrary> moduleStrategies = new();

    public DocumentationFromSymbolOrPath(Compilation compilation)
    {
        this.compilation = compilation;
    }

    public string LookupDocumentationFor(ISymbol symbol) =>
        LibraryForSymbol(symbol).LookupDocumentationFor(symbol);

    private IDocumentationLibrary LibraryForSymbol(ISymbol symbol)
    {
        var path = CandidateModulePath(symbol);
        if (path == null) return AlwaysUseSymbolMethod.Instance;
        if (moduleStrategies.TryGetValue(path, out var ret)) return ret;
        return SetStrategyForPath(path, DefaultStrategyForPath(path));
    }


    private static IDocumentationLibrary SetStrategyForPath(string path, IDocumentationLibrary strategy) =>
        moduleStrategies[path] = strategy;

    private IDocumentationLibrary DefaultStrategyForPath(string path) =>
        new DocumentationFromSymbolOrPath.TwoStrategyDocumentationLibrary(
            new XmlFileDocumentationLibrary(path), path);

    private string? CandidateModulePath(ISymbol symbol) =>
        compilation.GetMetadataReference(symbol.ContainingAssembly)?.Display;

    public IDocumentationLibrary OptimizedLibrary() => this;
}