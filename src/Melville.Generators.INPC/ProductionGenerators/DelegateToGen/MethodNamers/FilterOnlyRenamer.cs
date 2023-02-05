using System.Text.RegularExpressions;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;

public class FilterOnlyRenamer : IMethodNamer
{
    private readonly Regex filter;

    public FilterOnlyRenamer(string filterString)
    {
        filter = new Regex(filterString);
    }

    public string? ComputeNameFor(string sourceName) =>
        filter.IsMatch(sourceName) ? sourceName : null;
}