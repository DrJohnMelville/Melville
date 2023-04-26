using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.DocumentationCopiers;

public interface IDocumentationLibrary
{
    string LookupDocumentationFor(ISymbol symbol);
    IDocumentationLibrary OptimizedLibrary();
}