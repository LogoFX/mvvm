using System;
using System.Collections;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Contains extensions for <see cref="WrappingCollection"/>
    /// </summary>
    public static class WrappingCollectionExtensions
    {
        /// <summary>
        /// Assigns the specified data source to the specified <see cref="WrappingCollection"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The specified collection.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// collection
        /// or
        /// source
        /// </exception>
        public static T WithSource<T>(this T collection, IEnumerable source) where T : WrappingCollection
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (source == null)
                throw new ArgumentNullException(nameof(source));

            collection.AddSource(source);

            return collection;
        }
    }
}
