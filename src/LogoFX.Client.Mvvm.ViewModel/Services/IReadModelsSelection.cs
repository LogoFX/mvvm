using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents an object with read-only selection properties.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public interface IReadModelsSelection<out TItem>
    {
        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        IEnumerable<TItem> Selection { get; }

        /// <summary>
        /// Occurs when selection is changed.
        /// </summary>
        event EventHandler SelectionChanged;
    }
}
