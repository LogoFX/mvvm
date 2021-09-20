using System;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents object view model for the specified model.
    /// </summary>
    /// <typeparam name="T">The specified model.</typeparam>
    public class ObjectViewModel<T> : ObjectViewModel,IObjectViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        public ObjectViewModel()
            : this(default(T))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewModel&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ObjectViewModel(T model)
            : base(model)
        {}

        #region ObjectModel property

        /// <summary>
        /// ObjectModel property
        /// </summary>
        [Obsolete("Use Model instead")]
        public  T ObjectModel
        {
            get { return (T)base.InternalModel; }
        }

        /// <summary>
        /// Model property
        /// </summary>
        public new T Model
        {
            get
            {
                return (T)InternalModel;
            }
        }

        #endregion
    }
}
