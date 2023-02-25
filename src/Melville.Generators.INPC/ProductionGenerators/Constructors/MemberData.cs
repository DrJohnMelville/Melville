using Melville.Generators.INPC.GenerationTools.AstUtilities;

namespace Melville.Generators.INPC.ProductionGenerators.Constructors;

public record struct MemberData(
    string Type, string Name, string LowerCaseName, string? XmlDocummentation)
{
    public MemberData(string type, string name, string? XmlDocumentation) : 
        this(type, name, name.AsInstanceSymbol(), XmlDocumentation)
    {
    }
}