namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Implements a UWP version of the <see cref="IEditableCollectionView"/>.
    /// </summary>
    public interface IEditableCollectionView
    {
        /// <summary>
        /// Gets a value indicating whether a new instance can be added.
        /// </summary>
        /// <value>
        /// <c>true</c> if a new instance can be added; otherwise, <c>false</c>.
        /// </value>
        bool CanAddNew { get; }

        /// <summary>
        /// Gets a value indicating whether a new instance can be removed.
        /// </summary>
        /// <value>
        /// <c>true</c> if a new instance can be removed; otherwise, <c>false</c>.
        /// </value>
        bool CanRemove { get; }

        /// <summary>
        /// Gets a value indicating whether a new instance is being added.
        /// </summary>
        /// <value>
        /// <c>true</c> if a new instance is being added; otherwise, <c>false</c>.
        /// </value>
        bool IsAddingNew { get; }

        /// <summary>
        /// Gets the currently added item.
        /// </summary>
        /// <value>
        /// The currently added item.
        /// </value>
        object CurrentAddItem { get; }

        /// <summary>
        /// Adds the new item.
        /// </summary>
        /// <returns></returns>
        object AddNew();

        /// <summary>
        /// Cancels the new item editing.
        /// </summary>
        void CancelNew();

        /// <summary>
        /// Commits the new item editing.
        /// </summary>
        void CommitNew();

        /// <summary>
        /// Gets a value indicating whether the editing can be cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the editing can be cancelled; otherwise, <c>false</c>.
        /// </value>
        bool CanCancelEdit { get; }

        /// <summary>
        /// Gets a value indicating whether an item is being edited.
        /// </summary>
        /// <value>
        /// <c>true</c> if an item is being edited; otherwise, <c>false</c>.
        /// </value>
        bool IsEditingItem { get; }

        /// <summary>
        /// Gets the item that is currently being edited.
        /// </summary>
        /// <value>
        /// The item that is currently being edited.
        /// </value>
        object CurrentEditItem { get; }

        /// <summary>
        /// Starts item editing.
        /// </summary>
        /// <param name="item">The item.</param>
        void EditItem(object item);

        /// <summary>
        /// Cancels the edit.
        /// </summary>
        void CancelEdit();

        /// <summary>
        /// Commits the edit.
        /// </summary>
        void CommitEdit();
    }
}