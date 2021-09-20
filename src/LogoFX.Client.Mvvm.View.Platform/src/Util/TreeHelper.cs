using System.Collections.Generic;
using System.Linq;
#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Media;
#endif
#if WINDOWS_UWP || NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace LogoFX.Client.Mvvm.View.Util
{
    /// <summary>
    /// Logical and Visua tree helper.
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// Gets the visual descendant of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        public static T GetVisualDescendant<T>(this DependencyObject d)
        {
            return GetVisualDescendants<T>(d).FirstOrDefault();
        }

        /// <summary>
        /// Gets the visual descendants of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetVisualDescendants<T>(this DependencyObject d)
        {
            for (int n = 0; n < VisualTreeHelper.GetChildrenCount(d); n++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, n);

                if (child is T)
                {
                    yield return (T)(object)child;
                }

                foreach (T match in GetVisualDescendants<T>(child))
                {
                    yield return match;
                }
            }
        }

        /// <summary>
        /// Finds the visual ancestor of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="includeThis">if set to <c>true</c> [include this].</param>
        /// <returns></returns>
        public static T FindVisualAncestor<T>(this DependencyObject obj, bool includeThis) where T : DependencyObject
        {
            if (!includeThis)
                obj = VisualTreeHelper.GetParent(obj);

            while (obj != null && (!(obj is T)))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }

            return obj as T;
        }
#if NET || NETCORE || NETFRAMEWORK
        /// <summary>
        /// Finds the logical ancestor of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="includeThis">if set to <c>true</c> [include this].</param>
        /// <returns></returns>
        public static T FindLogicalAncestor<T>(this DependencyObject obj, bool includeThis) where T : DependencyObject
        {
            if (!includeThis)
                obj = LogicalTreeHelper.GetParent(obj);

            while (obj != null && (!(obj is T)))
            {
                obj = LogicalTreeHelper.GetParent(obj);
            }
            return obj as T;
        }
#endif
    }
}
