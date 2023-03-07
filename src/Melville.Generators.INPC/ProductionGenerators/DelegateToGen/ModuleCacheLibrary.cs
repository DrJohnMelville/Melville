using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public interface IDocumentationLibrary
{
    public string LookupDocumentationFor(ISymbol symbol);


}

public class ModuleCacheLibrary: IDocumentationLibrary
{
    private readonly Dictionary<string, IDocumentationLibrary> xmlProviders = new();
    public void RegisterAssemblies(ImmutableArray<MetadataReference> references)
    {
        foreach (var reference in references)
        {
            var key = AfterLastBackSlash(reference.Display??"");
            if (xmlProviders.ContainsKey(key)) continue;
            xmlProviders.Add(key, new SingleModuleLibrary(reference.Display ?? ""));
        }
    }

    private string AfterLastBackSlash(string referenceDisplay) => 
        referenceDisplay.Substring(referenceDisplay.LastIndexOf('\\')+1);


    public string LookupDocumentationFor(ISymbol symbol)
    {
        if (symbol.GetDocumentationCommentXml() is { Length: > 0 } native) return native;
        var module = symbol.ContainingModule;
        var displayString = module.ToDisplayString();
        return
            xmlProviders.TryGetValue(displayString, out var moduleLibrary) ? 
                moduleLibrary.LookupDocumentationFor(symbol) :"";
    }
}