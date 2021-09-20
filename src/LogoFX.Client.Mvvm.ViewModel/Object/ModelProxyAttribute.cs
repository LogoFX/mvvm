using System;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents proxy to the real property on the model.
    /// </summary>
    public class ModelProxyAttribute : Attribute
    {
        private string _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProxyAttribute"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public ModelProxyAttribute(string property)
        {
            _property = property;
        }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }
    }
}
