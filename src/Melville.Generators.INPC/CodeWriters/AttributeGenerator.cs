using System;
using System.Linq;

namespace Melville.Generators.INPC.CodeWriters;

public static class AttributeGenerator
{
    public static void SimpleAttribute(
        this CodeWriter cw, string name, AttributeTargets targets, bool allowMultiple = false) =>
        ComplexAttribute(cw, name, targets).Dispose();

    public static IDisposable ComplexAttribute(
        this CodeWriter cw, string name, AttributeTargets targets, bool allowMultiple = false)
    {
        cw.AddPrefixLine("using System;");
        cw.AppendLine($"[AttributeUsage({PrintAttributes(targets)}, Inherited=false{MultipleString(allowMultiple)})]");
        cw.AppendLine($"internal sealed class {name}Attribute: Attribute");
        return cw.CurlyBlock();
    }

    private static string MultipleString(bool allowMultiple) => allowMultiple ? ", AllowMultiple=true" : "";

    private static string PrintAttributes(AttributeTargets targets)
    {
        return string.Join(" | ",
            Enum.GetValues(typeof(AttributeTargets))
                .OfType<AttributeTargets>()
                .Where(i => (i & targets) == i)
                .Select(i => $"AttributeTargets.{Enum.GetName(typeof(AttributeTargets), i)}"));
    }
}