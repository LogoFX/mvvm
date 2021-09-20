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
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Selection notification event args.
    /// </summary>
    /// <typeparam name="T">Type of selected items.</typeparam>
    public class SelectionChangedEventArgs<T> : EventArgs
    {
        private readonly IEnumerable<T> _selection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="selection">The selection.</param>
        public SelectionChangedEventArgs(IEnumerable<T> selection)
        {
            _selection = selection;
        }

        /// <summary>
        /// Gets the selection.
        /// </summary>
        /// <value>The selection.</value>
        public IEnumerable<T> Selection
        {
            get { return _selection; }
        }
    }
}