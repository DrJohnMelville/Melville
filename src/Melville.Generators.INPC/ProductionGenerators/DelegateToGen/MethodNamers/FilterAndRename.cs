using System.Text.RegularExpressions;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;

public class FilterAndRename : IMethodNamer
{
    private readonly Regex filter;
    private readonly string replacement;
    public FilterAndRename(string filter, string replacement)
    {
        this.filter = new Regex(filter);
        this.replacement = replacement;
    }

    public string? ComputeNameFor(string sourceName)
    {
        var replaced = false;
        var ret = filter.Replace(sourceName, m =>
        {
            replaced = true;
            return m.Result(replacement);
        });
        return replaced ? ret : null;
    }
}