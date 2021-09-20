using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Simple implementation of the <see cref="ICollectionViewEx"/> interface, 
    /// which extends the standard WinRT definition of the <see cref="ICollectionView"/> 
    /// interface to add sorting and filtering.
    /// </summary>
    /// <remarks>
    /// Here's an example that shows how to use the <see cref="ListCollectionView"/> class:
    /// <code>
    /// // create a simple list
    /// var list = new List&lt;Rect&gt;();
    /// for (int i = 0; i &lt; 200; i++)
    ///   list.Add(new Rect(i, i, i, i));
    ///   
    /// // create collection view based on list
    /// var cv = new ListCollectionView();
    /// cv.Source = list;
    /// 
    /// // apply filter
    /// cv.Filter = (item) =&gt; { return ((Rect)item).X % 2 == 0; };
    /// 
    /// // apply sort
    /// cv.SortDescriptions.Add(new SortDescription("Width", ListSortDirection.Descending));
    /// 
    /// // show data on grid
    /// mygrid.ItemsSource = cv;
    /// </code>
    /// </remarks>   
    public class ListCollectionView :
        ICollectionViewEx,
        INotifyPropertyChanged,
        IComparer<object>                
    {
        //------------------------------------------------------------------------------------
        #region ** fields

        // original data source
        private object _source;
        // original data source as list
        private IList _sourceList;
        // listen to changes in the source
        private INotifyCollectionChanged _sourceNcc;
        // filtered/sorted data source
        private readonly List<object> _view;
        // sorting parameters
        private readonly ObservableCollection<SortDescription> _sort;
        // filter
        private Predicate<object> _filter;
        // cursor position
        private int _index;
        // suspend notifications
        private int _updating;

        #endregion

        //------------------------------------------------------------------------------------
        #region ** ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCollectionView"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public ListCollectionView(object source)
        {
            // view exposed to consumers
            _view = new List<object>();

            // sort descriptor collection
            _sort = new ObservableCollection<SortDescription>();
            _sort.CollectionChanged += _sort_CollectionChanged;

            // hook up to data source
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCollectionView"/> class.
        /// </summary>
        public ListCollectionView() 
            : this(null)
        { }

        #endregion

        //------------------------------------------------------------------------------------
        #region ** object model

        /// <summary>
        /// Gets or sets the collection from which to create the view.
        /// </summary>
        public object Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    // save new source
                    _source = value;

                    // save new source as list (so we can add/remove etc)
                    _sourceList = value as IList;

                    // listen to changes in the source
                    if (_sourceNcc != null)
                    {
                        _sourceNcc.CollectionChanged -= _sourceCollectionChanged;
                    }
                    _sourceNcc = _source as INotifyCollectionChanged;
                    if (_sourceNcc != null)
                    {
                        _sourceNcc.CollectionChanged += _sourceCollectionChanged;
                    }

                    // refresh our view
                    HandleSourceChanged();

                    // inform listeners
                    OnPropertyChanged("Source");
                }
            }
        }
        /// <summary>
        /// Update the view from the current source, using the current filter and sort settings.
        /// </summary>
        public void Refresh()
        {
            HandleSourceChanged();
        }
        /// <summary>
        /// Raises the <see cref="CurrentChanging"/> event.
        /// </summary>
        protected virtual void OnCurrentChanging(CurrentChangingEventArgs e)
        {
            if (_updating <= 0)
            {
                if (CurrentChanging != null)
                    CurrentChanging(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="CurrentChanged"/> event.
        /// </summary>
        protected virtual void OnCurrentChanged(object e)
        {
            if (_updating <= 0)
            {
                if (CurrentChanged != null)
                    CurrentChanged(this, e);
                OnPropertyChanged("CurrentItem");
            }
        }
        /// <summary>
        /// Raises the <see cref="VectorChanged"/> event.
        /// </summary>
        protected virtual void OnVectorChanged(IVectorChangedEventArgs e)
        {
            if (_updating <= 0)
            {
                if (VectorChanged != null)
                    VectorChanged(this, e);
                OnPropertyChanged("Count");
            }
        }
        /// <summary>
        /// Enters a defer cycle that you can use to merge changes to the view and delay
        /// automatic refresh.
        /// </summary>
        public IDisposable DeferRefresh()
        {
            return new DeferNotifications(this);
        }

        #endregion

        //------------------------------------------------------------------------------------
        #region ** event handlers

        // the original source has changed, update our source list
        void _sourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_updating <= 0)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems.Count == 1)
                        {
                            HandleItemAdded(e.NewStartingIndex, e.NewItems[0]);
                        }
                        else
                        {
                            HandleSourceChanged();
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems.Count == 1)
                        {
                            HandleItemRemoved(e.OldStartingIndex, e.OldItems[0]);
                        }
                        else
                        {
                            HandleSourceChanged();
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Reset:
                        HandleSourceChanged();
                        break;
                    default:
                        throw new Exception("Unrecognized collection change notification: " + e.Action);
                }
            }
        }

        // sort changed, refresh view
        void _sort_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_updating <= 0)
            {
                HandleSourceChanged();
            }
        }

        #endregion

        //------------------------------------------------------------------------------------
        #region ** implementation

        // add item to view
        void HandleItemAdded(int index, object item)
        {
            // if the new item is filtered out of view, no work
            if (_filter != null && !_filter(item))
            {
                return;
            }

            // compute insert index
            if (_sort.Count > 0)
            {
                // sorted: insert at sort position
                index = _view.BinarySearch(item, this);
                if (index < 0) index = ~index;
            }
            else if (_filter != null)
            {
                // if the source is not a list (e.g. enum), then do a full refresh
                if (_sourceList == null)
                {
                    HandleSourceChanged();
                    return;
                }

                // find insert index
                // count invisible items below the insertion point and
                // subtract from the number of items in the view
                // (counting from the bottom is more efficient for the
                // most common case which is appending to the source collection)
                var visibleBelowIndex = 0;
                for (int i = index; i < _sourceList.Count; i++)
                {
                    if (!_filter(_sourceList[i]))
                    {
                        visibleBelowIndex++;
                    }
                }
                index = _view.Count - visibleBelowIndex;
            }

            // add item to view
            _view.Insert(index, item);

            // keep selection on the same item
            if (index <= _index)
            {
                _index++;
            }

            // notify listeners
            var e = new VectorChangedEventArgs(CollectionChange.ItemInserted, index);
            OnVectorChanged(e);
        }

        // remove item from view
        void HandleItemRemoved(int index, object item)
        {
            // check if the item is in the view
            if (_filter != null && !_filter(item))
            {
                return;
            }

            // compute index into view
            if (index < 0 || index >= _view.Count || !Equals(_view[index], item))
            {
                index = _view.IndexOf(item);
            }
            if (index < 0)
            {
                return;
            }

            // notify listeners
            var e = new VectorChangedEventArgs(CollectionChange.ItemRemoved, index);
            OnVectorChanged(e);

            // remove item from view
            _view.RemoveAt(index);

            // keep selection on the same item
            if (index <= _index)
            {
                _index--;
            }
        }

        // update view after changes other than add/remove an item
        void HandleSourceChanged()
        {
            // keep selection if possible
            var currentItem = CurrentItem;

            // re-create view
            _view.Clear();
            var ie = Source as IEnumerable;
            if (ie != null)
            {
                foreach (var item in ie)
                {
                    if (_filter == null || _filter(item))
                    {
                        if (_sort.Count > 0)
                        {
                            var index = _view.BinarySearch(item, this);
                            if (index < 0) index = ~index;
                            _view.Insert(index, item);
                        }
                        else
                        {
                            _view.Add(item);
                        }
                    }
                }
            }

            // notify listeners
            OnVectorChanged(VectorChangedEventArgs.Reset);

            // restore selection if possible
            MoveCurrentTo(currentItem);
        }

        // update view after an item changes (apply filter/sort if necessary)

        // move the cursor to a new position
        bool MoveCurrentToIndex(int index)
        {
            // invalid?
            if (index < -1 || index >= _view.Count)
            {
                return false;
            }

            // no change?
            if (index == _index)
            {
                return false;
            }

            // fire changing
            var e = new CurrentChangingEventArgs();
            OnCurrentChanging(e);
            if (e.Cancel)
            {
                return false;
            }

            // change and fire changed
            _index = index;
            OnCurrentChanged(null);
            return true;
        }

        #endregion

        //------------------------------------------------------------------------------------
        #region ** nested classes

        /// <summary>
        /// Class that handles deferring notifications while the view is modified.
        /// </summary>
        class DeferNotifications : IDisposable
        {
            private readonly ListCollectionView _view;
            private readonly object _currentItem;

            internal DeferNotifications(ListCollectionView view)
            {
                _view = view;
                _currentItem = _view.CurrentItem;
                _view._updating++;
            }
            public void Dispose()
            {
                _view.MoveCurrentTo(_currentItem);
                _view._updating--;
                _view.Refresh();
            }
        }
        /// <summary>
        /// Class that implements IVectorChangedEventArgs so we can fire VectorChanged events.
        /// </summary>
        class VectorChangedEventArgs : IVectorChangedEventArgs
        {
            private readonly CollectionChange _cc;
            private readonly uint _index = (uint) 0xffff;

            private static readonly VectorChangedEventArgs _reset = new VectorChangedEventArgs(CollectionChange.Reset);
            public static VectorChangedEventArgs Reset
            {
                get { return _reset; }
            }

            public VectorChangedEventArgs(CollectionChange cc, int index = -1)
            {
                _cc = cc;
                _index = (uint)index;
            }
            public CollectionChange CollectionChange
            {
                get { return _cc; }
            }
            public uint Index
            {
                get { return _index; }
            }
        }

        #endregion

        //------------------------------------------------------------------------------------
        #region ** IComponentOneCollectionView adapter

        /// <summary>
        /// Gets a value that indicates whether this view supports filtering via the
        /// <see cref="ICollectionViewEx.Filter"/> property.
        /// </summary>
        public bool CanFilter { get { return true; } }

        /// <summary>
        /// Gets or sets a callback used to determine if an item is suitable for inclusion
        /// in the view.
        /// </summary>
        public Predicate<object> Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether this view supports grouping via the 
        /// <see cref="ICollectionViewEx.GroupDescriptions"/> property.
        /// </summary>
        public bool CanGroup { get { return false; } }

        /// <summary>
        /// Gets a collection of System.ComponentModel.GroupDescription objects that
        /// describe how the items in the collection are grouped in the view.
        /// </summary>
        public IList<object> GroupDescriptions { get { return null; } }

        /// <summary>
        /// Gets a value that indicates whether this view supports sorting via the 
        /// <see cref="ICollectionViewEx.SortDescriptions"/> property.
        /// </summary>
        public bool CanSort { get { return true; } }

        /// <summary>
        /// Gets a collection of System.ComponentModel.SortDescription objects that describe
        /// how the items in the collection are sorted in the view.
        /// </summary>
        public IList<SortDescription> SortDescriptions { get { return _sort; } }

        /// <summary>
        /// Get the underlying collection.
        /// </summary>
        public IEnumerable SourceCollection { get { return _source as IEnumerable; } }

        /// <summary>
        /// Gets or sets the custom sort comparer.
        /// </summary>
        /// <value>
        /// The custom sort comparer.
        /// </value>
        public IComparer CustomSort { get; set; }

        #endregion

        //------------------------------------------------------------------------------------
        #region ** ICollectionView

        /// <summary>
        /// Occurs after the current item has changed.
        /// </summary>
        public event EventHandler<object> CurrentChanged;
        /// <summary>
        /// Occurs before the current item changes.
        /// </summary>
        public event CurrentChangingEventHandler CurrentChanging;
        /// <summary>
        /// Occurs when the view collection changes.
        /// </summary>
        public event VectorChangedEventHandler<object> VectorChanged;
        /// <summary>
        /// Gets a colletion of top level groups.
        /// </summary>
        public IObservableVector<object> CollectionGroups
        {
            get { return null; }
        }
        /// <summary>
        /// Gets the current item in the view.
        /// </summary>
        public object CurrentItem
        {
            get { return _index > -1 && _index < _view.Count ? _view[_index] : null; }
            set { MoveCurrentTo(value); }
        }
        /// <summary>
        /// Gets the ordinal position of the current item in the view.
        /// </summary>
        public int CurrentPosition { get { return _index; } }

        /// <summary>
        /// Gets a value that indicates whether the CurrentItem of the view is beyond the end of the collection.
        /// </summary>
        /// <returns>
        /// true if the CurrentItem of the view is beyond the end of the collection; otherwise, false.
        /// </returns>
        public bool IsCurrentAfterLast { get { return _index >= _view.Count; } }

        /// <summary>
        /// Gets a value that indicates whether the CurrentItem of the view is beyond the beginning of the collection.
        /// </summary>
        /// <returns>
        /// true if the CurrentItem of the view is beyond the beginning of the collection; otherwise, false.
        /// </returns>
        public bool IsCurrentBeforeFirst { get { return _index < 0; } }

        /// <summary>
        /// Sets the first item in the view as the CurrentItem.
        /// </summary>
        /// <returns>
        /// true if the resulting CurrentItem is an item within the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToFirst() { return MoveCurrentToIndex(0); }

        /// <summary>
        /// Sets the last item in the view as the CurrentItem.
        /// </summary>
        /// <returns>
        /// true if the resulting CurrentItem is an item within the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToLast() { return MoveCurrentToIndex(_view.Count - 1); }

        /// <summary>
        /// Sets the item after the CurrentItem in the view as the CurrentItem.
        /// </summary>
        /// <returns>
        /// true if the resulting CurrentItem is an item within the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToNext() { return MoveCurrentToIndex(_index + 1); }

        /// <summary>
        /// Sets the item at the specified index to be the CurrentItem in the view.
        /// </summary>
        /// <returns>
        /// true if the resulting CurrentItem is an item within the view; otherwise, false.
        /// </returns>
        /// <param name="index">The index of the item to move to.</param>
        public bool MoveCurrentToPosition(int index) { return MoveCurrentToIndex(index); }

        /// <summary>
        /// Sets the item before the CurrentItem in the view as the CurrentItem.
        /// </summary>
        /// <returns>
        /// true if the resulting CurrentItem is an item within the view; otherwise, false.
        /// </returns>
        public bool MoveCurrentToPrevious() { return MoveCurrentToIndex(_index - 1); }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(object item) { return _view.IndexOf(item); }

        /// <summary>
        /// Sets the specified item to be the CurrentItem in the view.
        /// </summary>
        /// <returns>
        /// true if the resulting CurrentItem is within the view; otherwise, false.
        /// </returns>
        /// <param name="item">The item to set as the CurrentItem.</param>
        public bool MoveCurrentTo(object item) { return item != CurrentItem ? MoveCurrentToIndex(IndexOf(item)) : true; }

        // async operations not supported
        /// <summary>
        /// Gets a sentinel value that supports incremental loading implementations. See also LoadMoreItemsAsync.
        /// </summary>
        /// <returns>
        /// true if additional unloaded items remain in the view; otherwise, false.
        /// </returns>
        public bool HasMoreItems { get { return false; } }

        /// <summary>
        /// Initializes incremental loading from the view.
        /// </summary>
        /// <returns>
        /// The wrapped results of the load operation.
        /// </returns>
        /// <param name="count">The number of items to load.</param>
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            throw new NotSupportedException();
        }

        // list operations

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return _sourceList == null || _sourceList.IsReadOnly; }
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(object item)
        {
            CheckReadOnly();
            _sourceList.Add(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, object item)
        {
            CheckReadOnly();
            if (_sort.Count > 0 || _filter != null)
            {
                throw new Exception("Cannot insert items into sorted or filtered views.");
            }
            _sourceList.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            Remove(_view[index]);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(object item)
        {
            CheckReadOnly();
            _sourceList.Remove(item);
            return true;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            CheckReadOnly();
            _sourceList.Clear();
        }
        void CheckReadOnly()
        {
            if (IsReadOnly)
            {
                throw new Exception("The source collection cannot be modified.");
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public object this[int index]
        {
            get { return _view[index]; }
            set { _view[index] = value; }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(object item) { return _view.Contains(item); }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(object[] array, int arrayIndex)
        {
            _view.Cast<object>().ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count { get { return _view.Count; } }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<object> GetEnumerator() { return _view.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _view.GetEnumerator(); }

        #endregion
       
        //------------------------------------------------------------------------------------
        #region ** INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raises property change notification.
        /// </summary>
        /// <param name="propName"></param>
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        //------------------------------------------------------------------------------------
        #region ** IComparer

        int IComparer<object>.Compare(object x, object y)
        {
            if (CustomSort != null)
            {
                return CustomSort.Compare(x, y);
            }
            // compare two items
            foreach (var sd in _sort)
            {               
                var cx = sd.ValueGetter(x) as IComparable;
                var cy = sd.ValueGetter(y) as IComparable;

                try
                {
                    var cmp =
                        cx == cy ? 0 :
                        cx == null ? -1 :
                        cy == null ? +1 :
                        cx.CompareTo(cy);

                    if (cmp != 0)
                    {
                        return sd.Direction == ListSortDirection.Ascending ? +cmp : -cmp;
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("comparison failed...");
                }
            }
            return 0;
        }

        #endregion
    }
}