using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Xml;

namespace WpfWrapperGenerator
{
    public interface IRenderMethods
    {
        void SectionComment(string comment);

        void WriteGenericMethod(FieldInfo field, DependencyProperty dp,
            string methodName, Type targetType);

        void WriteInstanceMethod(FieldInfo field, DependencyProperty dp, string methodName, 
            Type targetType);
    }

    public class CodeWriter : IRenderMethods
    {
        private readonly StringBuilder writer = new StringBuilder();
        
        public override string ToString() => writer.ToString();

        public void RenderFileFooter()
        {
            writer.AppendLine("}");
            writer.AppendLine("}");
        }

        public void RenderFileHeadder()
        {
            writer.AppendLine("using Melville.Mvvm.CsXaml.ValueSource;");
            writer.AppendLine("namespace Melville.Mvvm.CsXaml.XamlBuilders {");
            writer.AppendLine("public static partial class WpfDeclarations {");
        }

        public void SectionComment(string comment)
        {
            writer.AppendLine();
            writer.Append("//");
            writer.AppendLine(comment);
        }
        
        public void WriteGenericMethod(FieldInfo field, DependencyProperty dp,
            string methodName, Type targetType)
        {
            string typeName = targetType.CSharpName();
            string dpType = dp.PropertyType.CSharpName();
            writer.Append($"public static TChild With{methodName}<TChild>(this TChild target, " +
                          $"ValueProxy<{dpType}>? value, " +
                          $"Disambigator<{typeName}, TChild>? doNotUse = null) where TChild: {typeName}");
            WriteBody(field);
        }



        public void WriteInstanceMethod(FieldInfo field, DependencyProperty dp, string methodName, 
            Type targetType)
        {
            string typeName = targetType.CSharpName();
            string dpType = dp.PropertyType.CSharpName();
            writer.Append($"public static {typeName} With{methodName}(this {typeName} target, " +
                          $"ValueProxy<{dpType}>? value) ");
            WriteBody(field);
        }

        private void WriteBody(FieldInfo field)
        {
            writer.AppendLine($"{{value?.SetValue(target, {StaticFieldCSharpName(field)}); return target;}}");
        }
        private static string StaticFieldCSharpName(FieldInfo field) => 
            $"{field.DeclaringType.CSharpName()}.{field.Name}";
    }
}