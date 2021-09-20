using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogoFX.Client.Mvvm.Model
{
    internal static class ReflectionExtensions
    {
        internal static IEnumerable<PropertyInfo> GetDeclaredTypeInfoProperties(this Type type) => type.GetProperties();

        internal static IEnumerable<PropertyInfo> GetRuntimeTypeInfoProperties(this Type type, BindingFlags flags) =>
            type.GetProperties(flags);

        internal static IEnumerable<Type> GetInterfaces(this Type type) => type.GetInterfaces();
    }
}
