using System;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents sort description.
    /// </summary>
    public class SortDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescription"/> class.
        /// </summary>
        /// <param name="valueGetter">The value getter.</param>
        /// <param name="direction">The direction.</param>
        public SortDescription(Func<object, object> valueGetter, ListSortDirection direction = ListSortDirection.Ascending)
        {            
            ValueGetter = valueGetter;
            Direction = direction;
        }

        /// <summary>
        /// Gets or sets the value getter.
        /// </summary>
        /// <value>
        /// The value getter.
        /// </value>
        public Func<object, object> ValueGetter { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public ListSortDirection Direction { get; set; }
    }
}
