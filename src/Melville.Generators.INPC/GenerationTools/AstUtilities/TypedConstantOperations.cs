using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class TypedConstantOperations
{
    public static string CodeString(this TypedConstant constant) =>
        constant.Value?.ToString() ?? "";
}