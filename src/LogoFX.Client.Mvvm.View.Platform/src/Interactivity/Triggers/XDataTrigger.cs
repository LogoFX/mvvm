using System.Windows;

namespace LogoFX.Client.Mvvm.View.Interactivity.Triggers
{
    /// <summary>
    /// Extended data trigger.
    /// </summary>
    public class XDataTrigger: Microsoft.Expression.Interactivity.Core.DataTrigger
    {
        #region RespectLoadedEvent dependency property

        /// <summary>
        /// Gets or sets the value indicating whether to respect the loaded event.
        /// </summary>
        public bool RespectLoadedEvent
        {
            get { return (bool) GetValue(RespectLoadedEventProperty); }
            set { SetValue(RespectLoadedEventProperty, value); }
        }

        /// <summary>
        /// Defines the dependency property holding the value which indicates whether to respect the loaded event.
        /// </summary>
        public static readonly DependencyProperty RespectLoadedEventProperty =
            DependencyProperty.Register("RespectLoadedEvent", typeof (bool), typeof (XDataTrigger), new PropertyMetadata(default(bool), OnRespectLoadedEventChanged));

        private static void OnRespectLoadedEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {}

        #endregion

        /// <inheritdoc />
        protected override void OnAttached()
        {   
            if(AssociatedObject is FrameworkElement && RespectLoadedEvent)
                ((FrameworkElement)AssociatedObject).Loaded += XDataTriggerLoaded;
            base.OnAttached();
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is FrameworkElement)
                ((FrameworkElement)AssociatedObject).Loaded -= XDataTriggerLoaded;
        }

        void XDataTriggerLoaded(object sender, RoutedEventArgs e)
        { //EvaluateBindingChange(null); 
        }
    }
}
