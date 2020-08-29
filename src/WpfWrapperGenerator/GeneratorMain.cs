using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfWrapperGenerator
{
    public class GeneratorMain
    {
        private readonly CodeWriter writer = new CodeWriter();
        public string Main()
        {  
            RenderAssembly(typeof(TextBlock).Assembly, typeof(UIElement).Assembly);
            return writer.ToString();
        }

        void RenderAssembly(params Assembly[] asm)
        {
            writer.RenderFileHeadder();
            var types = GetTypes(asm).ToList();
            RenderDependencyProperties(types);
            RenderRegularProperties(types);
            writer.RenderFileFooter();
        }

        private void RenderRegularProperties(List<Type> types)
        {
            foreach (var prop in RegularProperties(types))
            {
                writer.WriteNonDependencyProperty(prop);
            }
        }

        private static IEnumerable<PropertyInfo> RegularProperties(List<Type> types)
        {
            string[] banned = new string[]{"BitmapEffect", "BitmapEffectInput", "Item"};
            return types
                .SelectMany(i=>i.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(j=> !banned.Contains(j.Name) &&HasPublicSetter(j) &&
                          i.GetField(j.Name+"Property", BindingFlags.Static|BindingFlags.Public) == null));
        }

        private static bool HasPublicSetter(PropertyInfo j)
        {
            return j.GetAccessors().Any(k=>k.ReturnType == typeof(void));
        }

        private void RenderDependencyProperties(List<Type> types)
        {
            foreach (var propGroup in AllFields(types).GroupBy(i => i.Name))
            {
                writer.SectionComment(propGroup.Key);
                foreach (var prop in propGroup)
                {
                    RenderUniqueMethod(prop);
                }
            }
        }

        private void RenderUniqueMethod(FieldInfo first)
        {
            if (!(first.GetValue(null) is DependencyProperty dp)) return;
            StaticMethodRenderer.TryRender(first, dp, writer);
            InstanceMethodRenderer.TryRender(first, dp, writer );
        }

        public IEnumerable<FieldInfo> AllFields(IEnumerable<Type> types) =>
            types
                .SelectMany(PublicStaticFields)
                .Where(f => f.FieldType == typeof(DependencyProperty));

        private static IEnumerable<Type> GetTypes(Assembly[] asm)
        {
            return asm.SelectMany(i=>i.GetTypes()).Where(i => i.IsPublic);
        }

        private IEnumerable<FieldInfo> PublicStaticFields(Type i) => 
            i.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
    }
}