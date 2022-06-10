using Melville.Generators.INPC.GenerationTools.CodeWriters;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;

public static class DependencyPropertyCodeGenerator
{
    public static void Generate(this RequestParser rp, CodeWriter cw)
    {
        GenerateDependencyPropertyDeclaration(rp, cw);
    }

    private static void GenerateDependencyPropertyDeclaration(RequestParser rp, CodeWriter cw)
    {
        if (rp.Type == null) return;
        cw.AppendLine($"// {rp.PropName} Dependency Property Implementation");
        GenerateDependencyProperty(rp, cw);
        GenerateAccessorMembers(rp, cw);
        cw.AppendLine();
    }

    private static void GenerateDependencyProperty(RequestParser rp, CodeWriter cw)
    {
        if (rp.FromCustomDpDeclaration) return;
        cw.AppendLine($"public static readonly System.Windows.DependencyProperty {rp.DependencyPropName()} = ");
        var attached = rp.Attached ? "Attached" : "";
        cw.AppendLine($"    System.Windows.DependencyProperty.Register{attached}(");
        cw.AppendLine($"    \"{rp.PropName}\", typeof({TypeOfArgument(rp)}), typeof({rp.ParentType()}),");
        cw.AppendLine($"    new System.Windows.FrameworkPropertyMetadata({rp.DefaultExpression()}{ChangeFunc(rp)}));");
        cw.AppendLine();
    }

    private static string TypeOfArgument(RequestParser rp) => 
        rp.Type != null && rp.Type.IsValueType ? rp.NullableTargetType() : rp.TargetType();

    private static string ChangeFunc(RequestParser rp)
    {
        return ChangeFunctionFinder.ComputerChangeFunction(rp);
    }

    private static void GenerateAccessorMembers(RequestParser rp, CodeWriter cw)
    {
        if (rp.Attached)
        {
            GenerateAttachedHelperMethods(rp, cw);
        }
        else
        {
            GeneratePropertyDeclaration(rp, cw);
        }
    }

    private static void GeneratePropertyDeclaration(RequestParser rp, CodeWriter cw)
    {
        cw.AppendLine($"public {rp.NullableTargetType()} {rp.PropName}");
        using var block = cw.CurlyBlock();
        cw.AppendLine($"get => ({rp.NullableTargetType()})this.GetValue({rp.ParentType()}.{rp.DependencyPropName()});");
        cw.AppendLine($"set => this.SetValue({rp.ParentType()}.{rp.DependencyPropName()}, value);");
    }

    private static void GenerateAttachedHelperMethods(RequestParser rp, CodeWriter cw)
    {
        cw.AppendLine($"public static {rp.NullableTargetType()} Get{rp.PropName}(System.Windows.DependencyObject obj) =>");
        cw.AppendLine($"    ({rp.NullableTargetType()})obj.GetValue({rp.ParentType()}.{rp.DependencyPropName()});");
        cw.AppendLine($"public static void Set{rp.PropName}(System.Windows.DependencyObject obj, {rp.NullableTargetType()} value) =>");
        cw.AppendLine($"    obj.SetValue({rp.ParentType()}.{rp.DependencyPropName()}, value);");
    }
}