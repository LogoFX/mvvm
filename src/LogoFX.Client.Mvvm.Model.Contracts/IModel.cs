using System;
using System.ComponentModel;

namespace LogoFX.Client.Mvvm.Model.Contracts
{
    /// <summary>
    /// Represents model with basic support for property notifications and built-in Id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModel<T> : 
        INotifyPropertyChanged, 
        IHaveId<T>,
        IDataErrorInfo,    
        INotifyDataErrorInfo,
        IHaveErrors,
        IHaveExternalErrors
        where T:IEquatable<T>
    {
        /// <summary>
        /// Model name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Model description
        /// </summary>
        string Description { get; set; }              
    }

    /// <summary>
    /// Represents model with <see cref="int"/> as identifier.
    /// </summary>
    public interface IModel: IModel<int>
    {}
}