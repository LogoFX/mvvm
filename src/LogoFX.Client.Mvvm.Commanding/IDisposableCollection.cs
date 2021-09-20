using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents means of managing collection of <see cref="IDisposable"/>
    /// </summary>
    public interface IDisposableCollection : IDisposable
    {
        /// <summary>
        /// Gets the collection of <see cref="IDisposable"/>
        /// </summary>
        IEnumerable<IDisposable> Disposables { get; }

        /// <summary>
        /// Adds the disposable to the collection.
        /// </summary>
        /// <param name="disposable">The disposable.</param>
        void Add(IDisposable disposable);
    }
}