using System;
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
    /// <see cref="DependencyObject"/> based notifier on property change 
    /// </summary>
    public class NotificationHelperDp : DependencyObject
    {
        private readonly Action<object, object> _callback;
        private bool _isDetached = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHelperDp"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public NotificationHelperDp(Action<object, object> callback)
        {
            _callback = callback;
        }

        /// <summary>
        /// Gets or sets the bind value.
        /// </summary>
        /// <value>
        /// The bind value.
        /// </value>
        public object BindValue
        {
            get { return (object)GetValue(BindValueProperty); }
            set { SetValue(BindValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindValue.  This enables animation, styling, binding, etc...        
        /// <summary>
        /// The bind value property
        /// </summary>
        public static readonly DependencyProperty BindValueProperty =
            DependencyProperty.Register("BindValue", typeof(object), typeof(NotificationHelperDp),
                new PropertyMetadata(null, OnBindValueChanged));

        private static void OnBindValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NotificationHelperDp that = (NotificationHelperDp)d;

            if (!that._isDetached && that._callback != null
#if NET || NETCORE || NETFRAMEWORK
 && BindingOperations.IsDataBound(that, BindValueProperty)
#endif
)
                that._callback(e.NewValue, e.OldValue);
        }

        /// <summary>
        /// Clears the bound value.
        /// </summary>
        public void Detach()
        {
            _isDetached = true;
            this.ClearValue(BindValueProperty);
        }
    }
}