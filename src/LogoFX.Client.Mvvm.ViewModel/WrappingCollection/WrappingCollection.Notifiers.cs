using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    public partial class WrappingCollection
    {
        private readonly Dictionary<object, IIndexedDictionary<object, object>> _dictionary = new Dictionary<object, IIndexedDictionary<object, object>>();

        private NotifyCollectionChangedEventHandler _weakHandler;

        private object GetWrapper(object list, object model)
        {
            if (!_dictionary.ContainsKey(list) || !_dictionary[list].ContainsKey(model))
                return null;
            return _dictionary[list][model];
        }
        private void PutWrapper(object list, object model, object wrapper)
        {
            if (!_dictionary.ContainsKey(list))
                _dictionary.Add(list, _indexedDictionaryFactory.Create<object, object>());
            _dictionary[list].Add(model,wrapper);
        }

        private void PutWrapperAt(object list, object o, object wrapper, int index)
        {
            if (!_dictionary.ContainsKey(list))
                _dictionary.Add(list, _indexedDictionaryFactory.Create<object, object>());
            _dictionary[list].AddAt(index,o,wrapper);            
        }

        private void RemoveWrapper(object list, object model)
        {
            if (!_dictionary.ContainsKey(list) || !_dictionary[list].ContainsKey(model))
                return;
            _dictionary[list].Remove(model);
        }

        private object GetWrapperAt(object list, int index)
        {
            if (!_dictionary.ContainsKey(list))            
                _dictionary.Add(list, _indexedDictionaryFactory.Create<object, object>());            
            return _dictionary[list].Count>=index+1?_dictionary[list][index]:null;
        }

        private IIndexedDictionary<object, object> GetListWrappers(object list)
        {
            if (!_dictionary.ContainsKey(list))
                return _indexedDictionaryFactory.Create<object, object>();            
            return _dictionary[list];
        }       

        private void AddList(IEnumerable enumerable)
        {
            //make sure we catch collection changes
            IList<object> l;
            do
            {
                try
                {
                    l = enumerable.Cast<object>().ToList();
                }
                catch
                {
                   continue;                     
                }
                break;
            } while (true);

            l.ForEach(item => ListCollectionChanged(enumerable, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, -1)));

            if (enumerable is INotifyCollectionChanged notifyCollectionChanged)
            {
                if (_weakHandler == null)
                {
                    _weakHandler = WeakDelegate.From(ListCollectionChanged);
                }

                notifyCollectionChanged.CollectionChanged += _weakHandler;
            }
        }
        private void RemoveList(IEnumerable enumerable)
        {
            //make sure we catch collection changes
            IList<object> l;
            do
            {
                try
                {
                    l = enumerable.Cast<object>().ToList();
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);

            if (enumerable is INotifyCollectionChanged notifyCollectionChanged && _weakHandler != null)
                notifyCollectionChanged.CollectionChanged -= _weakHandler;

            l.ForEach(item => ListCollectionChanged(enumerable, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, -1)));
        }
        private void SourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems
                        .Cast<IEnumerable>()
                        .ForEach(AddList);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    e.OldItems
                        .Cast<IEnumerable>()
                        .ForEach(RemoveList);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    e.OldItems
                        .Cast<IEnumerable>()
                        .ForEach(RemoveList);
                    e.NewItems
                        .Cast<IEnumerable>()
                        .ForEach(AddList);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException("Clear() is not supported on Sources. Use Remove() instead.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Action<object> RemoveHandler = (a) =>
            {
                var wrapper = GetWrapper(sender, a);
                RemoveWrapper(sender, a);
                _collectionManager.Remove(wrapper);
                if (wrapper is IDisposable disposable)
                    disposable.Dispose();
            };
            Action<IEnumerable<object>> RemoveRangeHandler = collection =>
            {
                var wrappers = collection.Select(r => new Tuple<object, object>(GetWrapper(sender, r), r)).ToArray();
                wrappers.ForEach(a => RemoveWrapper(sender, a.Item2));
                _collectionManager.RemoveRange(wrappers.Select(t => t.Item1));
                wrappers.ForEach(wrapper =>
                {
                    if (wrapper is IDisposable disposable)
                        disposable.Dispose();
                });
            };
            Action<object> AddHandler = (a) =>
            {
                var wrapper = CreateWrapper(a);
                PutWrapper(sender, a, wrapper);
                _collectionManager.Add(wrapper);
            };
            Action<IEnumerable<object>> AddRangeHandler = collection =>
            {
                var wrapperPairs = collection.Select(a => Tuple.Create(a, CreateWrapper(a))).ToArray();
                wrapperPairs.ForEach(r => PutWrapper(sender, r.Item1, r.Item2));
                _collectionManager.AddRange(wrapperPairs.Select(t => t.Item2));
            };
            Action<object, int> InsertHandler = (a, index) =>
            {
                object wrapper = CreateWrapper(a);
                object oldWrapper = GetWrapperAt(sender, index);
                PutWrapperAt(sender, a, wrapper, index);
                if(oldWrapper!=null)
                {
                    int oldIndex = _collectionManager.IndexOf(oldWrapper);
                    _collectionManager.Insert(oldIndex, wrapper);
                }
                else
                {
                    _collectionManager.Add(wrapper);
                }
            };
            Action<IEnumerable<object>, int> InsertRangeHandler = (collection, index) =>
            {
                var initialIndex = index;
                var wrappers = collection.Select(a => Tuple.Create(a, CreateWrapper(a), initialIndex++)).ToArray();                                
                var rangeWrappers = new List<object>();
                wrappers.ForEach(r =>
                {
                    var oldWrapper = GetWrapperAt(sender, r.Item3);
                    var oldIndex = _collectionManager.IndexOf(oldWrapper);
                    PutWrapperAt(sender, r.Item1, r.Item2, r.Item3);
                    if (oldWrapper != null && oldIndex >= 0)
                    {
                        _collectionManager.Insert(oldIndex, r.Item2);                      
                    }
                    else
                    {
                        rangeWrappers.Add(r.Item2);
                    }
                });
                if (rangeWrappers.Count > 0)
                {
                    _collectionManager.AddRange(rangeWrappers);
                }
            };
           
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:                    
                    if (e.NewStartingIndex == -1)
                    {
                        Dispatch.Current.BeginOnUiThread(() => AddRangeHandler(e.NewItems.Cast<object>()));                        
                    }
                    else
                    {
                        Dispatch.Current.BeginOnUiThread(() =>
                        {
                            var newStartingIndex = e.NewStartingIndex;
                            InsertRangeHandler(e.NewItems.Cast<object>(), newStartingIndex);
                        });
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Dispatch.Current.BeginOnUiThread(() => RemoveRangeHandler(e.OldItems.Cast<object>()));
                    break;
                case NotifyCollectionChangedAction.Move:
                    Dispatch.Current.BeginOnUiThread(() =>
                    {
                        RemoveRangeHandler(e.OldItems.Cast<object>());

                        if (e.NewStartingIndex == -1)
                        {
                            Dispatch.Current.BeginOnUiThread(() => AddRangeHandler(e.NewItems.Cast<object>()));                            
                        }
                        else
                        {
                            Dispatch.Current.BeginOnUiThread(() =>
                            {
                                var newStartingIndex = e.NewStartingIndex;
                                InsertRangeHandler(e.NewItems.Cast<object>(), newStartingIndex);
                            });
                        }
                    });
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Dispatch.Current.BeginOnUiThread(() =>
                    {
                        RemoveRangeHandler(e.OldItems.Cast<object>());

                        if (e.NewStartingIndex == -1)
                        {
                            Dispatch.Current.BeginOnUiThread(() => AddRangeHandler(e.NewItems.Cast<object>()));
                        }
                        else
                        {
                            Dispatch.Current.BeginOnUiThread(() =>
                            {
                                int newStartingIndex = e.NewStartingIndex;
                                InsertRangeHandler(e.NewItems.Cast<object>(), newStartingIndex);
                            });
                        }
                    });
                    break;
                case NotifyCollectionChangedAction.Reset:
                    var listWrappers = GetListWrappers(sender);

                    Dispatch.Current.BeginOnUiThread(() =>
                    {
                        if (listWrappers.Count > 0)
                        {
                            RemoveRangeHandler(listWrappers.Select(a => a.Key).ToList());
                            listWrappers.Clear();                                                    
                        }                        
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
