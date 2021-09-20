using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LogoFX.Client.Mvvm.View.Converters
{
    /// <summary>
    /// Retrieves image by its name
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class ObjectToImageConverter : IValueConverter
    {
        private string _suffix;
        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string Extension
        {
            get { return _ext; }
            set { _ext = value; }
        }
        private string _folder = "Icons";
        private string _ext = ".png";
        private string _default;

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public string Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        /// <summary>
        /// Gets or sets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public string Default
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
            ImageGet.MakeRet mk = ImageGet.GetDelegateOnType(targetType);

            //IOperation c = value as IOperation;
            string folder = Folder;
            if (parameter != null && parameter is string && ((string)parameter).Trim().Length > 0)
                folder = (string) parameter;
            if (value != null)
            {
                try
                {
                    return mk(/*"pack://application:,,,/PG_Client;component/Images/" +*/ folder + "/"
                              + value +
                              (!String.IsNullOrEmpty(Suffix) ? ("_" + Suffix) : "")
                              + Extension);
                }
                catch (Exception)
                {
                    Debug.WriteLine("ImageConverter:" + value + " image is not found");
                }
            }
            return !string.IsNullOrWhiteSpace(Default)?mk(Default): DependencyProperty.UnsetValue;
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
        /// <exception cref="System.Exception">The method or operation is not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
