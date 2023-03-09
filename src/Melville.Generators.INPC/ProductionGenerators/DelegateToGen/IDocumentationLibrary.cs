using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public interface IDocumentationLibrary
{
    string LookupDocumentationFor(ISymbol symbol);
    IDocumentationLibrary OptimizedLibrary();
}