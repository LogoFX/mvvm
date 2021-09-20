using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.ViewModel
{
    interface IIndexedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {        
        void AddAt(int index, TKey key, TValue item);                
    }
}