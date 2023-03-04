using System;
using System.ComponentModel;

namespace Melville.INPC
{
    /// <summary>
    /// Interface that allows us to send notify messages on the auto generated INPC objects.
    /// </summary>
    public interface IExternalNotifyPropertyChanged:System.ComponentModel.INotifyPropertyChanged 
    {
        /// <summary>
        /// Sends a property change notification for a specific property name
        /// </summary>
        /// <param name="propertyName">The name of the propery that changed.</param>
        void OnPropertyChanged(string propertyName);
    }

    /// <summary>
    /// Helper methods surrounding registering for INotifyPropertyChange
    /// </summary>
    public static class PropertyChangeOperations
    {
        /// <summary>
        /// This extension method allows OnPropertyChanged to be called without a cast on objects that implement IExternalNotifyPropertyChange exp0licitly.
        /// </summary>
        /// <param name="obj">The object to send the notification</param>
        /// <param name="propertyName">The property that has changed.</param>
        public static void NotifyPropertyChange(this IExternalNotifyPropertyChanged obj, string propertyName) =>
            obj.OnPropertyChanged(propertyName);

        /// <summary>
        /// Monitor another INotifyPropertyChanged and signal one of my own properties changes whenever a foreign property changes.
        /// </summary>
        /// <param name="target">The object that will send additional property change messages.</param>
        /// <param name="foreignObject">The source object that will trigger those change messages.</param>
        /// <param name="foreignProperty">A specific property on the source object that triggers change messages.</param>
        /// <param name="localProperties">The properties on target that will send notification messages when the source property changes.</param>
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

        /// <summary>
        /// Monitor an INotifyProperty change and do a specific action whenever it changes.
        /// </summary>
        /// <param name="target">The object to monitor.</param>
        /// <param name="member">The member on target that should trigger the action.</param>
        /// <param name="action">The action to take upon property change notification.</param>
        /// <returns>An action that can be called to cancel the monitoring.</returns>
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
        /// <summary>
        /// Monitor an INotifyProperty change and do a specific action once, the first time the source property changes.
        /// </summary>
        /// <param name="target">The object to monitor.</param>
        /// <param name="member">The member on target that should trigger the action.</param>
        /// <param name="action">The action to take upon property change notification.</param>
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