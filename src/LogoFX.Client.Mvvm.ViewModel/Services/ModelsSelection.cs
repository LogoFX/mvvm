using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents models selection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ModelsSelection<TItem> : IReadModelsSelection<TItem>, IWriteModelsSelection<TItem>
    {
        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        public IEnumerable<TItem> Selection { get; private set; }

        /// <summary>
        /// Occurs when selection is changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Updates the selection.
        /// </summary>
        /// <param name="newSelection">The new selection.</param>
        public void UpdateSelection(IEnumerable<TItem> newSelection)
        {
            Selection = newSelection;
            SelectionChanged?.Invoke(this, new EventArgs());
        }
    }
}
