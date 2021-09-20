// ===================================
// <remarks>Part of this software based on various internet sources, mostly on the works
// of members of Wpf Disciples group http://wpfdisciples.wordpress.com/
// Also project may contain code from the frameworks: 
//        Nito 
//        OpenLightGroup
//        nRoute
// </remarks>
// ====================================================================================//

using System;
using System.Collections;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Simple type safe <c>ViewModel</c> list
    /// </summary>
    /// <typeparam name="THead">head model type</typeparam>
    /// <typeparam name="TChild">children model type</typeparam>
    public class SimpleObjectsListViewModel<THead,TChild>:ObjectsListViewModel,IObjectViewModel<THead>
    {        
        private readonly Func<TChild, IObjectViewModel<TChild>> _modelCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleObjectsListViewModel&lt;THead, TChild&gt;"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="models">The models.</param>
        /// <param name="creator">The creator.</param>
        public SimpleObjectsListViewModel(THead obj, IEnumerable models, Func<TChild, IObjectViewModel<TChild>> creator = null)
            : base(obj, models)
        {            
            _modelCreator = creator;
        }

        /// <summary>
        /// Gets the object model.
        /// </summary>
        /// <value>The object model.</value>
        public  virtual THead ObjectModel
        {
            get { return (THead)base.InternalModel; }
        }

        /// <summary>
        /// Gets the object model.
        /// </summary>
        /// <value>The object model.</value>
        public new virtual THead Model
        {
            get { return (THead)base.InternalModel; }
        }

        /// <summary>
        /// Creates view model
        /// </summary>
        /// <param name="parent">Parent model</param>
        /// <param name="obj">Object for which we making model</param>
        /// <returns></returns>
        public override IObjectViewModel CreateViewModel(IViewModel parent, object obj)
        {
            return   _modelCreator!=null?_modelCreator((TChild)obj):new ObjectViewModel<TChild>((TChild)obj);            
        }
    }
}
