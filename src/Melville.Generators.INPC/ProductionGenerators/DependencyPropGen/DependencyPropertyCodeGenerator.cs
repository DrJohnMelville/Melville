using Melville.Generators.INPC.GenerationTools.CodeWriters;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;


public static class DependencyPropertyCodeGeneratorOperatiopns
{
    public static void Generate(this RequestParser rp, CodeWriter cw)
    {
        new DependencyPropertyCodeGenerator(rp,cw).GenerateDependencyPropertyDeclaration();
    }
}
public readonly struct DependencyPropertyCodeGenerator
{
    private readonly RequestParser rp;
    private readonly CodeWriter cw;

    public DependencyPropertyCodeGenerator(RequestParser rp, CodeWriter cw)
    {
        this.rp = rp;
        this.cw = cw;
    }

    public void GenerateDependencyPropertyDeclaration()
    {
        if (rp.Type == null) return;
        cw.AppendLine($"// {rp.PropName} Dependency Property Implementation");
        GenerateDependencyProperty();
        GenerateAccessorMembers();
        cw.AppendLine();
    }

    private void GenerateDependencyProperty()
    {
        if (rp.FromCustomDpDeclaration) return;
        CopyTargetAttributes("field");
        cw.AppendLine($"public static readonly System.Windows.DependencyProperty {rp.DependencyPropName()} = ");
        var attached = rp.Attached ? "Attached" : "";
        cw.AppendLine($"    System.Windows.DependencyProperty.Register{attached}(");
        cw.AppendLine($"    \"{rp.PropName}\", typeof({TypeOfArgument()}), typeof({rp.ParentType()}),");
        cw.AppendLine($"    new System.Windows.FrameworkPropertyMetadata({rp.DefaultExpression()}{ChangeFunc()}));");
        cw.AppendLine();
    }

    private void CopyTargetAttributes(string attributePrefix)
    {
        new AttributeCopier(cw, attributePrefix).CopyAttributesFrom(rp.TargetMemberSyntax);
    }

    private string TypeOfArgument() => 
        rp.Type != null && rp.Type.IsValueType ? rp.NullableTargetType() : rp.TargetType();

    private string ChangeFunc() => ChangeFunctionFinder.ComputerChangeFunction(rp);

    private void GenerateAccessorMembers()
    {
        if (rp.Attached)
        {
            GenerateAttachedHelperMethods();
        }
        else
        {
            GeneratePropertyDeclaration();
        }
    }

    private void GeneratePropertyDeclaration()
    {
        CopyTargetAttributes("property");
        cw.AppendLine($"public {rp.NullableTargetType()} {rp.PropName}");
        using var block = cw.CurlyBlock();
        cw.AppendLine($"get => ({rp.NullableTargetType()})this.GetValue({rp.ParentType()}.{rp.DependencyPropName()});");
        cw.AppendLine($"set => this.SetValue({rp.ParentType()}.{rp.DependencyPropName()}, value);");
    }

    private void GenerateAttachedHelperMethods()
    {
        CopyTargetAttributes("method");
        cw.AppendLine($"public static {rp.NullableTargetType()} Get{rp.PropName}(System.Windows.DependencyObject obj) =>");
        cw.AppendLine($"    ({rp.NullableTargetType()})obj.GetValue({rp.ParentType()}.{rp.DependencyPropName()});");
        CopyTargetAttributes("method");
        cw.AppendLine($"public static void Set{rp.PropName}(System.Windows.DependencyObject obj, {rp.NullableTargetType()} value) =>");
        cw.AppendLine($"    obj.SetValue({rp.ParentType()}.{rp.DependencyPropName()}, value);");
    }
}