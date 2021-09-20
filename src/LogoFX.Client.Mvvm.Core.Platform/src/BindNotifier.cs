using System;
using System.Collections.Generic;
#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Data;
#endif
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace LogoFX.Client.Mvvm.Core
{
    /// <summary>
    /// Notifications observer
    /// </summary>
    public static class BindNotifier
    {
        static readonly WeakKeyDictionary<object, Dictionary<string, NotificationHelperDp>> _notifiers = new WeakKeyDictionary<object, Dictionary<string, NotificationHelperDp>>();

        /// <summary>
        /// Subscribes supplied object to property changed notifications and invokes the provided callback
        /// </summary>
        /// <typeparam name="T">Type of subject</typeparam>
        /// <param name="vmb">Subject</param>
        /// <param name="path">Property path</param>
        /// <param name="callback">Notification callback</param>
        public static void NotifyOn<T>(this T vmb, string path, Action<object, object> callback)
        {
            Dictionary<string, NotificationHelperDp> block;
            if (!_notifiers.TryGetValue(vmb, out block))
            {
                _notifiers.Add(vmb, block = new Dictionary<string, NotificationHelperDp>());
            }
            block.Remove(path);

            NotificationHelperDp binder = new NotificationHelperDp(callback);
            BindingOperations.SetBinding(binder, NotificationHelperDp.BindValueProperty,
#if NET || NETCORE || NETFRAMEWORK
 new Binding(path) { Source = vmb });
#else
            new Binding { Source = vmb,Path = new PropertyPath(path)});
#endif
            block.Add(path, binder);
        }

        /// <summary>
        /// Unsubscribes supplied object from property changed notifications
        /// </summary>
        /// <typeparam name="T">Type of subject</typeparam>
        /// <param name="vmb">Subject</param>
        /// <param name="path">Property path</param>
        public static void UnNotifyOn<T>(this T vmb, string path)
        {
            Dictionary<string, NotificationHelperDp> block;
            if (!_notifiers.TryGetValue(vmb, out block) || !block.ContainsKey(path))
            {
                return;
            }

            block[path].Detach();
            block.Remove(path);
        }
    }
}