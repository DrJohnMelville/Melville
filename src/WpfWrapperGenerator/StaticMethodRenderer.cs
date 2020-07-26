using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace WpfWrapperGenerator
{
    public static class StaticMethodRenderer
    {
        public static void TryRender(FieldInfo field, DependencyProperty dp, 
            CodeWriter writer)
        {
            if (!(FindSetMethod(dp.Name, field.DeclaringType) is {} method)) return;
            writer.WriteGenericMethod(field, dp, $"{field.DeclaringType?.Name}_{dp.Name}", 
                AttachedPropertyTargetType(method));
        }


        private static Type AttachedPropertyTargetType(MethodInfo method)
        {
            var setterType = StaticSetterTargetType(method);
            return typeof(DependencyObject).IsAssignableFrom(setterType)?
                setterType:typeof(DependencyObject);
        }

        private static Type StaticSetterTargetType( MethodInfo setterMethod) =>
            setterMethod
                .GetParameters()
                .FirstOrDefault()?
                .ParameterType ?? typeof(DependencyObject);

        private static MethodInfo? FindSetMethod(string name, Type type) =>
            type.GetMethod(PropNameToSetterName(name),
                BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);


        private static string PropNameToSetterName(string name) => "Set" +name;
   }
}