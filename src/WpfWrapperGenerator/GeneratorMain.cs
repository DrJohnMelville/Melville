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


        /*
        private void RenderNameGroup(List<FieldInfo> props)
        {
            var first = props.First();
            writer.SectionComment(first.Name);
            if (props.Count == 1)
            {
                RenderUniqueMethod(first);
                return;
            }

            var inheritenceGroups = props.GroupBy(i => i.GetValue(null));
            if (inheritenceGroups.Count() > 1)
            {
                RenderAllAsInstance(props);
            }
        }

        private void RenderAllAsInstance(List<FieldInfo> props)
        {
            foreach (var prop in props)
            {
                if ((prop.GetValue(null) is DependencyProperty dp)) 
                    InstanceMethodRenderer.TryRender(prop, dp, writer, true);
            }
        }
*/
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