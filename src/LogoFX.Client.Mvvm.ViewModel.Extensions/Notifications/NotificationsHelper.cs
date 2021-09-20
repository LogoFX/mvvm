using System;
using System.ComponentModel;
using System.Reactive.Linq;

// ReSharper disable once CheckNamespace
namespace LogoFX.Client.Mvvm.Notifications
{
    /// <summary>
    /// Notifications helper
    /// </summary>
    public static class NotificationsHelper
    {
        /// <summary>
        /// Subscribes supplied object to property changed notifications and invokes the provided callback        
        /// </summary>
        /// <typeparam name="T">Type of subject</typeparam><param name="subject">Subject</param><param name="path">Property path</param><param name="callback">Notification callback</param>
        public static void NotifyOn<T>(this T subject, string path, Action<object, object> callback)
            where T : INotifyPropertyChanged
        {
            Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                a => subject.PropertyChanged += a, a => subject.PropertyChanged -= a)
                .Where(a => a.EventArgs.PropertyName == path)
                .Subscribe(a => callback?.Invoke(new object(), new object()));
        }
    }
}
