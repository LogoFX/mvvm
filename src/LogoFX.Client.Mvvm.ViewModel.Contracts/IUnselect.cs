 namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents an object that can unselect an item or collection of items.
    /// </summary>
    public interface IUnselect
    {
        /// <summary>
        /// Deselects the specified item.
        /// </summary>
        /// <param name="item">The specified item.</param>
        /// <param name="notify">True, if the selection change should raise notification.</param>
        /// <returns>true if succeeded, otherwise false</returns>
        bool Unselect(object item, bool notify = true);

        /// <summary>
        /// Clears the selection.
        /// </summary>
        void ClearSelection();
    }
}
