namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;

public class NoRename : IMethodNamer
{
    public static readonly NoRename Instance = new();
    private NoRename(){}

    public string? ComputeNameFor(string sourceName) => sourceName;
}