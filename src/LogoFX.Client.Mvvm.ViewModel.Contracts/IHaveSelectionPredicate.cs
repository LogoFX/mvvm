using System;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents an object that has selection predicate.
    /// </summary>
    public interface IHaveSelectionPredicate
    {
        /// <summary>
        /// Gets or sets the selection predicate.
        /// </summary>
        Predicate<object> SelectionPredicate { get; set; }
    }
}