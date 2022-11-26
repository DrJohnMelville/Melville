using System.Text.RegularExpressions;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public static class StringModifier
{
    private static readonly Regex propertyNameRegex = new("_*(.)(.*)");

    public static string ComputePropertyName(this string fieldName)
    {
        var match = propertyNameRegex.Match(fieldName);
        return match.Groups[1].Value.ToUpper() + match.Groups[2].Value;
    }

}