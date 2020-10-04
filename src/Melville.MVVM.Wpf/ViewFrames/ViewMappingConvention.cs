using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.ViewFrames
{
    public interface IViewMappingConvention
    {
        UIElement ViewFromViewModel(object model, IDIIntegration diContainer);
    }
    public class ViewMappingConvention: IViewMappingConvention
    {
        public UIElement ViewFromViewModel(object model, IDIIntegration diContainer)
        {
            try
            {
                var viewTypeName = ViewTypeNameFromModel(model) ??
                   throw new InvalidOperationException($"{model.GetType()} does not contain \"ViewModel\".");
                var targetType = ViewTypeFromName(model, viewTypeName) ??
                    throw new InvalidOperationException($"Could Not Find Type {viewTypeName}.");
                return (CreateView(targetType, diContainer) is UIElement elt) ? elt :
                    DisplayMessage($"{viewTypeName} is not a UIElement.");
            }
            catch (InvalidOperationException e)
            {
                return DisplayMessage(e.Message);
            }
        }

        protected virtual Type? ViewTypeFromName(object model, string viewTypeName) => 
            model.GetType().Assembly.GetType(viewTypeName);

        protected virtual object CreateView(Type targetType, IDIIntegration di) =>
            di.Get(targetType) ??
            DisplayMessage("DI container failed to create type "+ targetType.Name);

        private static TextBlock DisplayMessage(string message) =>
            new TextBlock {Text = message, Foreground = Brushes.Red};

        protected virtual string? ViewTypeNameFromModel(object model)
        {
            var modelName = model.GetType().ToString();
            var nonGeneric = Regex.Match(modelName, @"^([A-Za-z\.0-9_\+]+)").Groups[1].Value;
            var viewTypeName = nonGeneric.Replace("ViewModel", "View");
            return modelName.Equals(viewTypeName, StringComparison.Ordinal) ? null : viewTypeName;
        }

    }
}