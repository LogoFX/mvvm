using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents an editable view model
    /// </summary>
    public interface IEditableViewModel
    {
        /// <summary>
        /// Returns <c>true</c> if the view model has changes, <c>false</c> otherwise. />
        /// </summary>
        bool IsDirty { get; }
        /// <summary>
        /// Returns <c>true</c> if the view model has errors, <c>false</c> otherwise. />
        /// </summary>
        bool HasErrors { get; }
        /// <summary>
        /// Gets or sets the value which enables/disables undo operations.
        /// </summary>
        bool CanUndo { get; set; }
        
        /// <summary>
        /// Reverts the last operation.
        /// </summary>
        void Undo();

        /// <summary>
        /// Saves the state asynchronously
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveAsync();        
    }
}
