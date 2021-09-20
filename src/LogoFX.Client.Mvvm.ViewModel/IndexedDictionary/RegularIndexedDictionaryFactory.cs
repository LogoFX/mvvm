namespace LogoFX.Client.Mvvm.ViewModel
{
    class RegularIndexedDictionaryFactory : IIndexedDictionaryFactory
    {
        public IIndexedDictionary<TKey, TValue> Create<TKey, TValue>()
        {
            return new WrappingCollection.IndexedDictionary<TKey, TValue>();
        }
    }
}