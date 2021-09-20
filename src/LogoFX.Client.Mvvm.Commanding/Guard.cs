using System;
using System.Diagnostics;
using System.Globalization;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Contains static guard clauses
    /// </summary>
    public static class Guard
    {
        private const string PARAMETER_NOT_VALID = "The parameter '{0}' value is not valid";
        private const string PARAMETER_NOT_NULLOREMPTY = "The parameter '{0}' cannot be null or empty";
        private const string PARAMETER_MUSTBE_OFTYPE = "The parameter '{0}' must be of type '{1}";
        
        /// <summary>
        /// Asserts the argument is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerStepThrough]       
        public static void ArgumentNotNull<T>(T value, string parameterName)
            where T
		        : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Asserts the argument is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T value, string parameterName, string message)
            where T
                : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        /// <summary>
        /// Asserts the argument is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T value, string parameterName, string messageFormat, params object[] messageArgs)
            where T
                : class
        {
            ArgumentNotNull(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        /// <summary>
        /// Asserts the argument does not have default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNotDefault<T>(T value, string parameterName)
        {
            if (object.Equals(value, default(T)))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Asserts the argument does not have default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNotDefault<T>(T value, string parameterName, string message)
        {
            if (object.Equals(value, default(T)))
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        /// <summary>
        /// Asserts the argument does not have default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotDefault<T>(T value, string parameterName, string messageFormat, params object[] messageArgs)
        {
            ArgumentNotDefault(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        /// <summary>
        /// Asserts the argument is neither null nor empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(string value, string parameterName)
        {
            ArgumentNotNullOrEmpty(value, parameterName, string.Format(CultureInfo.CurrentCulture,
                PARAMETER_NOT_NULLOREMPTY, parameterName));
        }

        /// <summary>
        /// Asserts the argument is neither null nor empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(string value, string parameterName, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(message, parameterName);
            }
        }

        /// <summary>
        /// Asserts the argument is neither null nor empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNullOrEmpty(string value, string parameterName, string messageFormat, params object[] messageArgs)
        {
            ArgumentNotNullOrEmpty(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        /// <summary>
        /// Asserts the argument is neither null nor whitespace.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNullOrWhiteSpace(string value, string parameterName)
        {
            ArgumentNotNullOrWhiteSpace(value, parameterName, string.Format(CultureInfo.CurrentCulture,
                    PARAMETER_NOT_NULLOREMPTY, parameterName));
        }

        /// <summary>
        /// Asserts the argument is neither null nor whitespace.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentNotNullOrWhiteSpace(string value, string parameterName, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(message, parameterName);
            }
        }

        /// <summary>
        /// Asserts the argument is neither null nor whitespace.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNullOrWhiteSpace(string value, string parameterName, string messageFormat, params object[] messageArgs)
        {
            ArgumentNotNullOrWhiteSpace(value, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        /// <summary>
        /// Asserts the argument is not out of range.
        /// </summary>
        /// <param name="outOfRange">if set to <c>true</c> [out of range].</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(bool outOfRange, string parameterName)
        {
            if (outOfRange)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        /// <summary>
        /// Asserts the argument is not out of range.
        /// </summary>
        /// <param name="outOfRange">if set to <c>true</c> [out of range].</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(bool outOfRange, string parameterName, string message)
        {
            if (outOfRange)
            {
                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }

        /// <summary>
        /// Asserts the argument is not out of range.
        /// </summary>
        /// <param name="outOfRange">if set to <c>true</c> [out of range].</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(bool outOfRange, string parameterName, string messageFormat, params object[] messageArgs)
        {
            ArgumentOutOfRange(outOfRange, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        /// <summary>
        /// Asserts the argument is from the given type.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void ArgumentIsType(object argument, Type type, string parameterName)
        {
            ArgumentIsType(argument, type, parameterName, string.Format(CultureInfo.CurrentCulture,
                    PARAMETER_MUSTBE_OFTYPE, parameterName, type.FullName));
        }

        /// <summary>
        /// Asserts the argument is from the given type.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentIsType(object argument, Type type, string parameterName, string message)
        {
            if (argument == null || !type.IsInstanceOfType(argument))
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Asserts the argument is from the given type.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentIsType(object argument, Type type, string parameterName, string messageFormat,
            params object[] messageArgs)
        {
            ArgumentIsType(argument, type, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }

        /// <summary>
        /// Throws exception for the given parameter.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        public static void ArgumentValue(bool throwException, string parameterName)
        {
            ArgumentValue(throwException, parameterName, string.Format(CultureInfo.CurrentCulture,
                PARAMETER_NOT_VALID, parameterName));
        }

        /// <summary>
        /// Throws exception for the given parameter.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException"></exception>
        [DebuggerStepThrough]
        public static void ArgumentValue(bool throwException, string parameterName, string message)
        {
            if (throwException)
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>
        /// Throws exception for the given parameter.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="messageArgs">The message arguments.</param>
        [DebuggerStepThrough]
        public static void ArgumentValue(bool throwException, string parameterName, string messageFormat, params object[] messageArgs)
        {
            ArgumentValue(throwException, parameterName, string.Format(CultureInfo.CurrentCulture, messageFormat, messageArgs));
        }
    }
}