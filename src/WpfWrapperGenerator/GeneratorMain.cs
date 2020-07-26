using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfWrapperGenerator
{
    public class GeneratorMain
    {
        public string Main()
        {
            var writer = new StringBuilder();
            var type = typeof(TextBlock);
            RenderAssembly(type.Assembly, writer);
            return writer.ToString();
        }

        void RenderAssembly(Assembly asm, StringBuilder writer)
        {
            RenderFileHeadder(writer);
            foreach (var prop in AllFields(asm))
            {
                RenderProperty(prop, writer);
            }
            RenderFileFooter(writer);
        }

        public IEnumerable<FieldInfo> AllFields(Assembly asm) =>
            asm
                .GetTypes().Where(i => i.IsPublic)
                .SelectMany(PublicStaticFields)
                .Where(f => f.FieldType == typeof(DependencyProperty));

        private IEnumerable<FieldInfo> PublicStaticFields(Type i) => 
            i.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        private static void RenderFileFooter(StringBuilder writer)
        {
            writer.AppendLine("}");
            writer.AppendLine("}");
        }

        private static void RenderFileHeadder(StringBuilder writer)
        {
            writer.AppendLine("using Melville.Mvvm.CsXaml.ValueSource;");
            writer.AppendLine("namespace Melville.Mvvm.CsXaml.XamlBuilders {");
            writer.AppendLine("public static partial class WpfDeclarations {");
        }

        void RenderProperty(FieldInfo field, StringBuilder writer)
        {
            var targetType = TargetType(field);
            if (!typeof(DependencyObject).IsAssignableFrom(targetType))
            {
                targetType = typeof(DependencyObject);
            }
            var typeName = targetType.CSharpName();
            
            if (!(field.GetValue(null) is DependencyProperty prop)) return;
            if (prop.ReadOnly) return;
            writer.Append("public static ");
            writer.Append(typeName);
            writer.Append(" With");
            if (targetType != field.DeclaringType)
            {
                writer.Append(field.DeclaringType?.Name ??"");
                writer.Append("_");
            }
            writer.Append(prop.Name);
            writer.Append("(this ");
            writer.Append(typeName);
            writer.Append(" target, ValueProxy<");
            writer.Append(prop.PropertyType.CSharpName());
            writer.Append(">? propValue) ");
            writer.Append(" {propValue?.SetValue(target, ");
            writer.Append(field.DeclaringType.CSharpName());
            writer.Append(".");
            writer.Append(field.Name);
            writer.AppendLine("); return target;}");
        }

        public Type TargetType(FieldInfo field)
        {
            return FindSetMethod(field.Name, field.DeclaringType??typeof(DependencyObject))?
                       .GetParameters().FirstOrDefault()?
                       .ParameterType ?? field.DeclaringType ?? typeof(DependencyObject);
        }
        
        public MethodInfo? FindSetMethod(string name, Type type) =>
            type.GetMethod(PropNameToSetterName(name),
                BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);


        string PropNameToSetterName(string name) => "Set" + RemoveProperty(name);
        string RemoveProperty(string name) => name.EndsWith("Property") ? name[..^8] : name;

// Define other methods, classes and namespaces here
    }
}