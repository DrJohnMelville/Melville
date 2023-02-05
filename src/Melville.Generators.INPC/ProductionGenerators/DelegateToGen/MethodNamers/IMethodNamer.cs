using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MethodNamers;

public interface IMethodNamer
{
    string? ComputeNameFor(string sourceName);
}