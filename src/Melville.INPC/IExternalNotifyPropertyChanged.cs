using System;
using System.ComponentModel;

namespace Melville.INPC
{
    public interface IExternalNotifyPropertyChanged:System.ComponentModel.INotifyPropertyChanged 
    {
        void OnPropertyChanged(string propertyName);
    }

    public static class PropertyChangeOperations
    {
        public static void DelegatePropertyChangeFrom(
            this IExternalNotifyPropertyChanged target,
            INotifyPropertyChanged foreignObject,
            string foreignProperty, 
            params string[] localProperties)
        {
            PropertyChangedEventHandler foreignObjectOnPropertyChanged = (s, e) =>
            {
                if (e.PropertyName.Equals(foreignProperty, StringComparison.Ordinal))
                {
                    foreach (var localProperty in localProperties)
                    {
                        target.OnPropertyChanged(localProperty);
                    }
                }
            };
            foreignObject.PropertyChanged += foreignObjectOnPropertyChanged;
        }
        public static Action WhenMemberChanges(this INotifyPropertyChanged target, string member, Action action)
        {
            PropertyChangedEventHandler method = (s, e) =>
            {
                if (e.PropertyName.Equals(member, StringComparison.Ordinal))
                {
                    action();
                }
            };
            target.PropertyChanged += method;

            return () => target.PropertyChanged -= method;
        }
        public static void WhenMemberChangesOnce(this INotifyPropertyChanged target, string member, Action action)
        {
            void method(object s, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals(member, StringComparison.Ordinal))
                {
                    action();
                    target.PropertyChanged -= method;
                }
            };
            target.PropertyChanged += method;
        }

    }
}