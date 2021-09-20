using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Represents view models collection.
    /// </summary>
    /// <typeparam name="T">The type of view model item.</typeparam>
    public interface IViewModelsCollection<out T> : IEnumerable<T>, INotifyPropertyChanged, INotifyCollectionChanged where T : IViewModel
    {
        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>       
        /// <param name="index">The index.</param>
        /// <returns></returns>
        T this[int index] { get; }
    }

    /// <summary>
    /// Represents view models collection where each item is an object view model.
    /// </summary>
    public interface IObjectViewModelsCollection : IViewModelsCollection<IObjectViewModel>
    {
    }

    /// <summary>
    /// Represents view models collection where each item inherits from object view model.
    /// </summary>
    /// <typeparam name="T">The type of view model item.</typeparam>
    public interface IObjectViewModelsCollection<out T> : IViewModelsCollection<T> where T : IObjectViewModel
    {
    }
}