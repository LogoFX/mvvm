namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents a view model which wraps around an enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumEntryViewModel<T>:ObjectViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumEntryViewModel{T}"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        public EnumEntryViewModel(T obj):base(obj)
        {}        
    }
}
