using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public class DictionaryLibrary : IDocumentationLibrary
{
    private readonly IReadOnlyDictionary<string, string> members;

    public DictionaryLibrary(IReadOnlyDictionary<string, string> members)
    {
        this.members = members;
    }

    public string LookupDocumentationFor(ISymbol symbol)
    {
        var name = symbol.GetDocumentationCommentId() ?? "xml";
        return members.TryGetValue(name, out var ret) ? ret : "";
    }

    public IDocumentationLibrary OptimizedLibrary() => this;
}