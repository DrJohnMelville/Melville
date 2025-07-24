using System;
using Melville.Generators.INPC.GenerationTools.CodeWriters;

namespace Melville.Generators.INPC.ProductionGenerators.DependencyPropGen;


public class BindablePropertyCodeGenerator(RequestParser rp, CodeWriter cw) : 
    AbstractPropertyCodeGenerator(rp, cw, 
        "global::Microsoft.Maui.Controls.BindableObject",
        "global::Microsoft.Maui.Controls.BindableProperty", "Create")
{
    public override void RenderDefaultAndChangeMethod()
    {
        cw.Append("    ");
        cw.Append(rp.DefaultExpression());
        cw.Append(MauiChangeFunctionFinder.ComputeChangeFunction(rp));
    }
}
public class DependencyPropertyCodeGenerator(RequestParser rp, CodeWriter cw) : AbstractPropertyCodeGenerator(rp, cw,
    "global::System.Windows.DependencyObject", 
    "global::System.Windows.DependencyProperty", "Register")
{
    public override void RenderDefaultAndChangeMethod()
    {
        cw.Append($"    new global::System.Windows.FrameworkPropertyMetadata({rp.DefaultExpression()}{ChangeFunc()})");
    }

    private string ChangeFunc() => WpfChangeFunctionFinder.ComputeChangeFunction(rp);

}
public abstract class AbstractPropertyCodeGenerator(
    RequestParser rp, CodeWriter cw, string objType, string propertyType, 
    string createMethod)
{
    protected readonly RequestParser rp = rp;
    protected readonly CodeWriter cw = cw;

    public void GenerateDependencyPropertyDeclaration()
    {
        if (rp.Type == null) return;
        GenerateDependencyProperty();
        GenerateAccessorMembers();
        cw.AppendLine();
    }

    private void GenerateDependencyProperty()
    {
        if (rp.FromCustomDpDeclaration) return;
        RenderXmlDocComment("field");
        CopyTargetAttributes("field");
        cw.AppendLine($"public static readonly {propertyType} {rp.DependencyPropName()} = ");
        var attached = rp.Attached ? "Attached" : "";
        cw.AppendLine($"    {propertyType}.{createMethod}{attached}(");
        cw.AppendLine($"    \"{rp.PropName}\", typeof({TypeOfArgument()}), typeof({rp.ParentType()}),");
        RenderDefaultAndChangeMethod();
        cw.AppendLine(");");
        cw.AppendLine();
    }

    public abstract void RenderDefaultAndChangeMethod();

    private void CopyTargetAttributes(string attributePrefix) =>
        new AttributeCopier(cw, attributePrefix)
            .CopyAttributesFrom(rp.TargetMemberSyntax);

    private string TypeOfArgument() => 
        rp.Type != null && rp.Type.IsValueType ? rp.NullableTargetType() : rp.TargetType();

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
        RenderXmlDocComment("property");
        CopyTargetAttributes("property");
        cw.AppendLine($"public {rp.NullableTargetType()} {rp.PropName}");
        using var block = cw.CurlyBlock();
        cw.AppendLine($"get => ({rp.NullableTargetType()})this.GetValue(global::{rp.ParentType()}.{rp.DependencyPropName()});");
        cw.AppendLine($"set => this.SetValue(global::{rp.ParentType()}.{rp.DependencyPropName()}, value);");
    }

    private void GenerateAttachedHelperMethods()
    {
        RenderXmlDocComment("getter");
        CopyTargetAttributes("method");
        cw.AppendLine($"public static {rp.NullableTargetType()} Get{rp.PropName}({objType} obj) =>");
        cw.AppendLine($"    ({rp.NullableTargetType()})obj.GetValue(global::{rp.ParentType()}.{rp.DependencyPropName()});");


        RenderXmlDocComment("setter");
        CopyTargetAttributes("method");
        cw.AppendLine($"public static void Set{rp.PropName}({objType} obj, {rp.NullableTargetType()} value) =>");
        cw.AppendLine($"    obj.SetValue(global::{rp.ParentType()}.{rp.DependencyPropName()}, value);");
    }

    private void RenderXmlDocComment(string kind)
    {
        cw.AppendLine("/// <summary>");
        if (LacksExplicitDocumentation())
            RenderGenericDocumentation(kind);
        else
            RenderCustomDocumentation();
        cw.AppendLine("/// </summary>");
    }

    private bool LacksExplicitDocumentation()
    {
        return string.IsNullOrWhiteSpace(rp.XmlDocumentation);
    }

    private void RenderGenericDocumentation(string kind) => 
        cw.AppendLine($"/// DependencyProperty {kind} for {rp.PropName}");

    private void RenderCustomDocumentation()
    {
        foreach (var str in DocumentationLines())
        {
            cw.Append("/// ");
            cw.AppendLine(str);
        }
    }

    private string[] DocumentationLines() => 
        rp.XmlDocumentation.Split(
            new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
}