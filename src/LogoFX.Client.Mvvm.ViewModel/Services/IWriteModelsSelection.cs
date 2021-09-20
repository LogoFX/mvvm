using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel.Services
{
    /// <summary>
    /// Represents an object which enables changing selection.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public interface IWriteModelsSelection<in TItem>
    {
        /// <summary>
        /// Updates the selection.
        /// </summary>
        /// <param name="newSelection">The new selection.</param>
        void UpdateSelection(IEnumerable<TItem> newSelection);
    }
}
