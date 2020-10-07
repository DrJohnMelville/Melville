using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Melville.MVVM.Wpf.DiParameterSources
{
    public class DiIBinding: MarkupExtension
    {
        private readonly Type? desiredType;

        public DiIBinding()
        {
        }

        public DiIBinding(Type? desiredType)
        {
            this.desiredType = desiredType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueTarget = GetValueTarget(serviceProvider);
            return DiIntegration
                .SearchForContainer(valueTarget.TargetObject as DependencyObject)
                .Get(FindType(valueTarget)) ??
                throw new InvalidOperationException("Dependency Injection Failed");
        }

        private static IProvideValueTarget GetValueTarget(IServiceProvider serviceProvider) => (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));

        private Type FindType(IProvideValueTarget serviceProvider) =>
            desiredType??
            serviceProvider.TargetProperty switch
            {
                PropertyInfo pi => pi.PropertyType,
                DependencyProperty dp => dp.PropertyType,  
                MethodInfo mi when mi.GetParameters().Length == 2 => // attached property
                    mi.GetParameters()[1].ParameterType,
                _ => throw new InvalidDataException("Can only assign to a property or dependency property")
            };
    }
}