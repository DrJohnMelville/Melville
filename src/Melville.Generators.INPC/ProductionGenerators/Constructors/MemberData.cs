using Melville.Generators.INPC.GenerationTools.AstUtilities;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

public record struct MemberData(string Type, string Name, string LowerCaseName)
{
    public MemberData(string type, string name) : this(type, name, name.AsInstanceSymbol())
    {
    }
}