using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.ViewModel
{
    partial class WrappingCollection
    {
        private static class CollectionManagerFactory
        {
            internal static ICollectionManager CreateRegularManager()
            {
                return new RegularCollectionManager();
            }

            internal static ICollectionManager CreateRangeManager()
            {
                return new RangeCollectionManager();
            }            
        }

        private class BeforeClearEventArgs : EventArgs
        {
            public BeforeClearEventArgs(IEnumerable<object> items)
            {
                Items = items;
            }

            public IEnumerable<object> Items { get; private set; }
        }

        private interface ICollectionManager
        {
            event EventHandler<BeforeClearEventArgs> BeforeClear;
            INotifyCollectionChanged CollectionChangedSource { get; }
            IEnumerator GetEnumerator();
            int ItemsCount { get; }
            void Add(object item);
            void AddRange(IEnumerable<object> items);
            void Remove(object item);
            void RemoveRange(IEnumerable<object> items);
            int IndexOf(object item);
            void Insert(int index, object item);
            object First();
            bool Contains(object item);
            object Find(object item);
            IList AsList();
        }

        private class RegularCollectionManager : ICollectionManager
        {
            private readonly ObservableCollection<object> _items = new ObservableCollection<object>();
            
            /// <summary>
            /// This event is raised before the contents of the collection are cleared.
            /// </summary>
            public event EventHandler<BeforeClearEventArgs> BeforeClear;

            public INotifyCollectionChanged CollectionChangedSource
            {
                get { return _items; }
            }

            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            public int ItemsCount
            {
                get { return _items.Count; }
            }

            public void Add(object item)
            {
                _items.Add(item);
            }

            public void AddRange(IEnumerable<object> items)
            {
                items.ForEach(_items.Add);
            }

            public void Remove(object item)
            {
                _items.Remove(item);
            }

            public void RemoveRange(IEnumerable<object> items)
            {
                BeforeClear?.Invoke(this , new BeforeClearEventArgs(items));
                items.ForEach(r => _items.Remove(r));
            }

            public int IndexOf(object item)
            {
                return _items.IndexOf(item);
            }

            public void Insert(int index, object item)
            {
                _items.Insert(index, item);                
            }

            public object First()
            {
                return _items[0];
            }

            public bool Contains(object item)
            {
                return _items.Contains(item);
            }

            public object Find(object item)
            {
                return _items.FirstOrDefault(t => t.Equals(item));
            }

            public IList AsList()
            {
                return _items;
            }
        }

        private class RangeCollectionManager : ICollectionManager
        {
            private readonly RangeObservableCollection<object> _items = new RangeObservableCollection<object>();

            public event EventHandler<BeforeClearEventArgs> BeforeClear = delegate { };

            public INotifyCollectionChanged CollectionChangedSource
            {
                get { return _items; }
            }

            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            public int ItemsCount
            {
                get { return _items.Count; }
            }

            public void Add(object item)
            {
                _items.Add(item);
            }

            public void AddRange(IEnumerable<object> items)
            {
                _items.AddRange(items);
            }

            public void Remove(object item)
            {
                _items.Remove(item);
            }

            public void RemoveRange(IEnumerable<object> items)
            {
                _items.RemoveRange(items, en => { BeforeClear(this, new BeforeClearEventArgs(en)); });
            }

            public int IndexOf(object item)
            {
                return _items.IndexOf(item);
            }

            public void Insert(int index, object item)
            {
                _items.Insert(index, item);
            }

            public object First()
            {
                return _items[0];
            }

            public bool Contains(object item)
            {
                return _items.Contains(item);
            }

            public object Find(object item)
            {
                return _items.FirstOrDefault(t => t.Equals(item));
            }

            public IList AsList()
            {
                return _items;
            }
        }

        /// <summary>
        /// Returns the contents as <see cref="IList"/>.
        /// </summary>
        /// <returns></returns>
        public IList AsList()
        {
            return _collectionManager.AsList();
        }
    }
}
