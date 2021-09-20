using System;
using System.Windows;

namespace LogoFX.Client.Mvvm.View.Controls
{
    /// <summary>
    /// Represents a function which conveys information about property change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="RoutedPropertyChangingEventArgs{T}"/> instance containing the event data.</param>
    public delegate void RoutedPropertyChangingEventHandler<T>(object sender, RoutedPropertyChangingEventArgs<T> e);

    /// <summary>
    /// Event arguments for property change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Windows.RoutedEventArgs" />
    public class RoutedPropertyChangingEventArgs<T> : RoutedEventArgs
    {
        private bool _cancel;

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public DependencyProperty Property { get; private set; }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public T OldValue { get; private set; }

        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public T NewValue { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is cancelable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is cancelable; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelable { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoutedPropertyChangingEventArgs{T}"/> is cancel.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancel; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.InvalidOperationException">invalid cancel</exception>
        public bool Cancel
        {
            get
            {
                return this._cancel;
            }
            set
            {
                if (this.IsCancelable)
                    this._cancel = value;
                else if (value)
                    throw new InvalidOperationException("invalid cancel");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether in coercion.
        /// </summary>
        /// <value>
        ///   <c>true</c> if in coercion; otherwise, <c>false</c>.
        /// </value>
        public bool InCoercion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedPropertyChangingEventArgs{T}"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="isCancelable">if set to <c>true</c> [is cancelable].</param>
        public RoutedPropertyChangingEventArgs(DependencyProperty property, T oldValue, T newValue, bool isCancelable)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.IsCancelable = isCancelable;
            this.Cancel = false;
        }

#if NET || NETCORE || NETFRAMEWORK
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedPropertyChangingEventArgs{T}"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="isCancelable">if set to <c>true</c> [is cancelable].</param>
        /// <param name="routedEvent">The routed event.</param>
        public RoutedPropertyChangingEventArgs(DependencyProperty property, T oldValue, T newValue, bool isCancelable, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            this.Property = property;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.IsCancelable = isCancelable;
            this.Cancel = false;
        }
#endif
    }
}
