namespace LogoFX.Client.Mvvm.ViewModel
{
    class ConcurrentIndexedDictionaryFactory : IIndexedDictionaryFactory
    {
        public IIndexedDictionary<TKey, TValue> Create<TKey, TValue>()
        {
            return new WrappingCollection.ConcurrentIndexedDictionary<TKey, TValue>();
        }
    }
}