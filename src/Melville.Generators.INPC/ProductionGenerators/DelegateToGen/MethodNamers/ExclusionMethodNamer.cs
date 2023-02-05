using System.Text.RegularExpressions;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;

public class ExclusionMethodNamer : IMethodNamer
{
    private readonly IMethodNamer innerNamer;
    private Regex exclusionRule;

    public ExclusionMethodNamer(IMethodNamer innerNamer, string? exclude)
    {
        this.innerNamer = innerNamer;
        exclusionRule = new Regex(exclude);
    }

    public string? ComputeNameFor(string sourceName) => 
        exclusionRule.IsMatch(sourceName)?null: innerNamer.ComputeNameFor(sourceName);
}