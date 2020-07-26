using System;
using System.Reflection;
using System.Windows;

namespace WpfWrapperGenerator
{
    public static class InstanceMethodRenderer
    {
        public static void TryRender(FieldInfo field, DependencyProperty dp,
            IRenderMethods writer)
        {
            if (!(ClassHasGetterForThisValue(field, dp) &&
                  field.DeclaringType is {} propertyHostType &&
                  IsADependencyObject(propertyHostType))) return;
            if (propertyHostType.IsSealed)
            {
                writer.WriteInstanceMethod(field, dp, dp.Name, propertyHostType);
            }
            else
            {
                writer.WriteGenericMethod(field, dp, dp.Name, propertyHostType);
                
            }
        }

        private static bool IsADependencyObject(Type propertyHostType)
        {
            return typeof(DependencyObject).IsAssignableFrom(propertyHostType);
        }

        private static bool ClassHasGetterForThisValue(FieldInfo field, DependencyProperty dp) => 
            GetProperty(dp.Name, field.DeclaringType) is {};

        private static PropertyInfo? GetProperty(string name, Type? type) =>
            type?.GetProperty(name);
    }
}