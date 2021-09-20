using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Mvvm.View.Utils
{
    /// <summary>
    /// Enables storing and querying the specified enumeration type.
    /// </summary>
    public static class EnumHelper
    {
        private static readonly IDictionary<Type, object[]> Cache = new Dictionary<Type, object[]>();

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static object GetBoxed(Enum s)
        {
            Type enumType = s.GetType();
            object ret = GetValues(enumType).FirstOrDefault(ss => ss.ToString() == s.ToString());
            return ret;
        }

        /// <summary>
        /// Gets all values of the specified enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Type ' + enumType.Name + ' is not an enum</exception>
        public static T[] GetValues<T>()
        {
            Type enumType = typeof(T);

            if (enumType.GetTypeInfo().IsEnum == false)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            object[] values;
            if (!Cache.TryGetValue(enumType, out values))
            {
                values = (from field in enumType.GetRuntimeFields()
                    where field.IsLiteral
                    select field.GetValue(enumType)).ToArray();
                Cache[enumType] = values;
            }
            return values.Cast<T>().ToArray();
        }

        /// <summary>
        /// Gets all values of the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Type ' + enumType.Name + ' is not an enum</exception>
        public static object[] GetValues(Type enumType)
        {
            if (enumType.GetTypeInfo().IsEnum == false)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            object[] values;
            if (!Cache.TryGetValue(enumType, out values))
            {
                values = (from field in enumType.GetRuntimeFields()
                    where field.IsLiteral
                    select field.GetValue(enumType)).ToArray();
                Cache[enumType] = values;
            }
            return values;
        }
    }
}