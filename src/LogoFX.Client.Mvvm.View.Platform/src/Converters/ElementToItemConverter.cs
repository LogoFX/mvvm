using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Intended to map ItemsControl's item to some other item inside converters Items list
    /// Usecase: providing distinct brush per item index
    /// </summary>
    public class ElementToItemConverter : IValueConverter
    {
        private object _default;
        private readonly List<object> _items = new List<object>();

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public List<object> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets or sets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public object Default
        {
            get { return _default; }
            set { _default = value; }
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DependencyObject item = value as DependencyObject;

            ItemsControl view = ItemsControl.ItemsControlFromItemContainer(item);

            int i;
            try
            {
                i = view.ItemContainerGenerator.IndexFromContainer(item);
            }
            catch (Exception)
            {
                return Default;
            }
            return _items[i%_items.Count];
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
