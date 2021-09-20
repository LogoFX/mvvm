using System.Windows;

namespace LogoFX.Client.Mvvm.View.DependencyProperties
{
    /// <summary>
    /// Adjusts the associated <see cref="FrameworkElement"/>'s size />
    /// </summary>
    public static class SizeObserver
    {
        /// <summary>
        /// True when the size should be observed initially, false otherwise.
        /// </summary>
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(SizeObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));

        /// <summary>
        /// Initial observed width.
        /// </summary>
        public static readonly DependencyProperty ObservedWidthProperty = DependencyProperty.RegisterAttached(
            "ObservedWidth",
            typeof(double),
            typeof(SizeObserver));

        /// <summary>
        /// Initial observed height.
        /// </summary>
        public static readonly DependencyProperty ObservedHeightProperty = DependencyProperty.RegisterAttached(
            "ObservedHeight",
            typeof(double),
            typeof(SizeObserver));

        /// <summary>
        /// Gets the value indicating whether the size should be observed.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns></returns>
        public static bool GetObserve(FrameworkElement frameworkElement)
        {           
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }

        /// <summary>
        /// Sets the value indicating whether the size should be observed.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <param name="observe">True when the size should be observed, false otherwise.</param>
        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {         
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        /// <summary>
        /// Gets the observed width.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns></returns>
        public static double GetObservedWidth(FrameworkElement frameworkElement)
        {         
            return (double)frameworkElement.GetValue(ObservedWidthProperty);
        }

        /// <summary>
        /// Sets the observed width.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <param name="observedWidth">The width.</param>
        public static void SetObservedWidth(FrameworkElement frameworkElement, double observedWidth)
        {         
            frameworkElement.SetValue(ObservedWidthProperty, observedWidth);
        }

        /// <summary>
        /// Gets the observed height.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns></returns>
        public static double GetObservedHeight(FrameworkElement frameworkElement)
        {            
            return (double)frameworkElement.GetValue(ObservedHeightProperty);
        }

        /// <summary>
        /// Sets the observed height.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <param name="observedHeight">The height.</param>
        public static void SetObservedHeight(FrameworkElement frameworkElement, double observedHeight)
        {            
            frameworkElement.SetValue(ObservedHeightProperty, observedHeight);
        }

        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;

            if ((bool)e.NewValue)
            {
                frameworkElement.SizeChanged += OnFrameworkElementSizeChanged;
                UpdateObservedSizesForFrameworkElement(frameworkElement);
            }
            else
            {
                frameworkElement.SizeChanged -= OnFrameworkElementSizeChanged;
            }
        }

        private static void OnFrameworkElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateObservedSizesForFrameworkElement((FrameworkElement)sender);
        }

        private static void UpdateObservedSizesForFrameworkElement(FrameworkElement frameworkElement)
        {           
            frameworkElement.SetCurrentValue(ObservedWidthProperty, frameworkElement.ActualWidth);
            frameworkElement.SetCurrentValue(ObservedHeightProperty, frameworkElement.ActualHeight);           
        }
    }
}