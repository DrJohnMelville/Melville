using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

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
            foreach (var propGroup in AllFields(asm).GroupBy(i=>i.Name))
            {
                writer.SectionComment(propGroup.Key);
                foreach (var prop in propGroup)
                {
                    RenderUniqueMethod(prop);
                }
            }
            writer.RenderFileFooter();
        }

        private void RenderUniqueMethod(FieldInfo first)
        {
            if (!(first.GetValue(null) is DependencyProperty dp)) return;
            StaticMethodRenderer.TryRender(first, dp, writer);
            InstanceMethodRenderer.TryRender(first, dp, writer );
        }

        public IEnumerable<FieldInfo> AllFields(Assembly[] asm) =>
            asm.SelectMany(i=>i.GetTypes()).Where(i => i.IsPublic)
                .SelectMany(PublicStaticFields)
                .Where(f => f.FieldType == typeof(DependencyProperty));

        private IEnumerable<FieldInfo> PublicStaticFields(Type i) => 
            i.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
    }
}