using System;
using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model
{
    internal interface IErrorInfoExtractionStrategy
    {
        IEnumerable<string> ExtractChildrenErrors(Type type, object propertyContainer);
        IEnumerable<string> GetPropertyInfoSources(Type type);
        bool IsPropertyErrorInfoSource(Type type, string propertyName);        
        object GetErrorInfoSourceValue<T>(Type type, string propertyName, Model<T> model) where T : IEquatable<T>;
    }
}