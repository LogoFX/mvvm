namespace LogoFX.Client.Mvvm.ViewModel
{
    interface IIndexedDictionaryFactory
    {
        IIndexedDictionary<TKey, TValue> Create<TKey, TValue>();
    }
}