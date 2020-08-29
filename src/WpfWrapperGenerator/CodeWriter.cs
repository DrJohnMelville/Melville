using System;
using System.Data;
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

        void RenderMethod(FieldInfo field, DependencyProperty dp,
            string methodName, Type targetType);

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

        public void RenderMethod(FieldInfo field, DependencyProperty dp,
            string methodName, Type targetType)
        {
            string typeName = targetType.CSharpName();
            WriteMethodDeclaration(dp, methodName, targetType, typeName, "{0}");
            WriteBody(field);
            WriteMethodDeclaration(dp, methodName, targetType, typeName, "Style<{0}>");
            WriteStyleBody(field);
        }

        private void WriteMethodDeclaration(DependencyProperty dp, string methodName, Type targetType, string typeName, string formatStr)
        {
            if (targetType.IsSealed)
            {
                var finalTypeName = string.Format(formatStr, typeName);
                string dpType = dp.PropertyType.CSharpName();
                writer.Append($"public static {finalTypeName} With{methodName}(this {finalTypeName} target, " +
                              $"ValueProxy<{dpType}>? value) ");
            }
            else
            {
                var finalTypeName = string.Format(formatStr, "TChild");
                writer.Append($"public static {finalTypeName} With{methodName}<TChild>(this {finalTypeName} target, " +
                              CreateValueProxy(dp.PropertyType) + "? value, " +
                              $"Disambigator<{typeName}, TChild>? doNotUse = null) where TChild: {typeName}");
            }
        }

        private static string CreateValueProxy(Type dpType)
        {
            if (dpType == typeof(Thickness)) return "ThicknessValueProxy";
            return $"ValueProxy<{dpType.CSharpName()}>";
        }


        private void WriteBody(FieldInfo field) => writer.AppendLine($"{{value?.SetValue(target, {StaticFieldCSharpName(field)}); return target;}}");
        private void WriteStyleBody(FieldInfo field) => writer.AppendLine($"{{value?.StyleSetter(target, {StaticFieldCSharpName(field)}); return target;}}");

        private static string StaticFieldCSharpName(FieldInfo field) => 
            $"{field.DeclaringType.CSharpName()}.{field.Name}";

        public void WriteNonDependencyProperty(PropertyInfo prop)
        {
            writer.AppendLine($"// {prop.DeclaringType} / {prop.Name}");
            string typeName = prop.DeclaringType.CSharpName();
            if (prop.PropertyType?.FullName == null) return;
            var dpType = PropertyTypeString(prop);
            if (prop.DeclaringType?.IsSealed ?? true)
            {
                writer.AppendLine($"public static {typeName} With{prop.Name}<TChild>(this {typeName} target, " +
                                  dpType + " value) ");
            }
            else
            {
                writer.AppendLine($"public static TChild With{prop.Name}<TChild>(this TChild target, " +
                                  dpType + " value, " +
                                  $"Disambigator<{typeName}, TChild>? doNotUse = null) where TChild: {typeName}");
            }

            writer.AppendLine($"{{if (value != null) target.{prop.Name} = value ?? default; return target; }}");

        }

        private static string PropertyTypeString(PropertyInfo prop)
        {
            string dpType = prop.PropertyType.CSharpName();
            dpType += dpType.StartsWith("System.Nullable<") ? "" : "?";
            return dpType;
        }
    }
}