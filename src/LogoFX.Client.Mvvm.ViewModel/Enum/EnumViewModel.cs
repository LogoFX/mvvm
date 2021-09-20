using System.Linq;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents a view model which wraps around a collection of enum values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumViewModel<T> : IHierarchicalViewModel
    {
        private ObservableViewModelsCollection<IObjectViewModel> _children;

        /// <summary>
        /// Returns an enum model wrapper for specified enum value.
        /// </summary>
        /// <param name="item">The specified enum value.</param>
        public EnumEntryViewModel<T> this[T item]
        {
            get { return InternalChildren.Cast<EnumEntryViewModel<T>>().First(a => a.Model.Equals(item)); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumViewModel{T}"/> class.
        /// </summary>
        public EnumViewModel()
        {
            EnumHelper.GetValues<T>().ForEach(a => InternalChildren.Add(new EnumEntryViewModel<T>(a)));
        }

        #region Implementation of IHierarhicalViewModel

        /// <summary>
        /// Gets the children.
        /// </summary>
        public IViewModelsCollection<IObjectViewModel> Children
        {
            get { return InternalChildren; }
        }

        /// <summary>
        /// Gets the items.(GLUE:compatibility to caliburn micro)
        /// </summary>
        public IViewModelsCollection<IObjectViewModel> Items
        {
            get { return InternalChildren; }
        }

        private ObservableViewModelsCollection<IObjectViewModel> InternalChildren
        {
            get { return _children?? (_children = new ObservableViewModelsCollection<IObjectViewModel>()); }
        }

        #endregion
    }
}
