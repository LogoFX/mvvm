using System;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Adds functionality to <see cref="Type"/> reflection.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified object is an instance of the provided type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool IsInstanceOfType(this Type type, object obj)
        {
            return obj != null && type.GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo());
        }
        /// <summary>
        /// Determines whether one type is assignable from another.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static bool IsAssignableFrom(this Type type, Type t)
        {
            return type.GetTypeInfo().IsAssignableFrom(t.GetTypeInfo());
        }
    }
}