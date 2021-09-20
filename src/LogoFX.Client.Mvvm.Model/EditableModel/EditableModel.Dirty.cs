using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;
using Solid.Patterns.Memento;

namespace LogoFX.Client.Mvvm.Model
{
    partial class EditableModel<T>
    {
        [NonSerialized]
        private Checkpoint _checkPoint;

        private sealed class Checkpoint
        {
            private readonly LogoFX.Core.WeakReference<IMemento<EditableModel<T>>> _memento;

            public Checkpoint(EditableModel<T> model)
            {                
                _memento = LogoFX.Core.WeakReference<IMemento<EditableModel<T>>>.Create(model._history.PeekUndo());
            }

            public bool Equals(IMemento<EditableModel<T>> other)
            {
                if (_memento == null || _memento.IsAlive == false)
                {
                    return other == null;
                }
                return _memento.Target.Equals(other);
            }
        }

        /// <summary>
        /// Represents an API for subscribing and unsubscribing to inner property notifications
        /// </summary>
        private interface IInnerChangesSubscriber
        {
            void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate, Action isCanCancelChangesChangedDelegate);
            void UnsubscribeToNotifyingObjectChanges(object notifyingObject);
        }

        //TODO
        /// <summary>
        /// An implementation of inner changes subscriber which is based on explicit INPC subscription
        /// This implementation does NOT use Weak Delegates internally due to the 
        /// fact that such an implementation fails to work and it is therefore necessary
        /// to explicitly unsubscribe from the notifications - potential source of leaks
        /// </summary>
        private class PropertyChangedInnerChangesSubscriber : IInnerChangesSubscriber
        {
            private readonly WeakKeyDictionary<object, Tuple<Action, Action>> _handlers = new WeakKeyDictionary<object, Tuple<Action, Action>>();            

            public void SubscribeToNotifyingObjectChanges(object notifyingObject, Action isDirtyChangedDelegate,
                Action isCanCancelChangesChangedDelegate)
            {
                if (_handlers.ContainsKey(notifyingObject) == false)
                {
                    _handlers.Add(notifyingObject, new Tuple<Action, Action>(isDirtyChangedDelegate, isCanCancelChangesChangedDelegate));
                }
                if (notifyingObject is INotifyPropertyChanged propertyChangedSource)
                {
                    propertyChangedSource.PropertyChanged += PropertyChangedSourceOnPropertyChanged;                                              
                }
            }

            public void UnsubscribeToNotifyingObjectChanges(object notifyingObject)
            {
                if (notifyingObject is INotifyPropertyChanged propertyChangedSource)
                {
                    propertyChangedSource.PropertyChanged -= PropertyChangedSourceOnPropertyChanged;
                    _handlers.Remove(notifyingObject);
                }
            }

            private void PropertyChangedSourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
            {
                if (_handlers.ContainsKey(sender))
                {
                    var delegates = _handlers[sender];
                    if (propertyChangedEventArgs.PropertyName == "IsDirty")
                    {
                        delegates.Item1.Invoke();
                    }
                    if (propertyChangedEventArgs.PropertyName == "CanCancelChanges")
                    {
                        delegates.Item2.Invoke();
                    }
                }                
            }
        }

        [NonSerialized]
        private readonly IInnerChangesSubscriber _innerChangesSubscriber = new PropertyChangedInnerChangesSubscriber();

        [NonSerialized]
        private bool _isOwnDirty;
        /// <inheritdoc />
        public virtual bool IsDirty => OwnDirty || SourceValuesAreDirty() || SourceCollectionsAreDirty();

        private bool _canCancelChanges = true;        
        /// <inheritdoc />
        public bool CanCancelChanges
        {
            get => _canCancelChanges && IsDirty;
            set
            {
                if (_canCancelChanges == value)
                {
                    return;
                }

                _canCancelChanges = value;
                NotifyOfPropertyChange();
            }
        }

        private bool SourceValuesAreDirty()
        {
            return
                TypeInformationProvider.GetDirtySourceValuesUnboxed(Type, this)
                    .Any(dirtySource => dirtySource != null && dirtySource.IsDirty);
        }        

        private bool SourceCollectionsAreDirty()
        {
            return
                TypeInformationProvider.GetDirtySourceCollectionsUnboxed(Type, this)
                    .Any(dirtySource => dirtySource != null && dirtySource.IsDirty);
        }

        /// <summary>
        /// This state is used to store the information about the Model's own Dirty state
        /// The overall Dirty state is influenced by this value as well as by the singular and collections Dirty states
        /// </summary>
        private bool OwnDirty
        {
            get => _isOwnDirty;
            set
            {
                _isOwnDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
                NotifyOfPropertyChange(() => CanCancelChanges);
            }
        }

        /// <inheritdoc />
        public void CancelChanges()
        {
            RestoreFromHistory();
            CancelProperties();
            CancelCollections();
        }

        private void CancelProperties()
        {
            var cancelChangesProperties = TypeInformationProvider.GetCanCancelChangesSourceValuesUnboxed(Type, this);
            foreach (var cancelChangesProperty in cancelChangesProperties.Where(x => x!= null &&  x.CanCancelChanges))
            {
                cancelChangesProperty.CancelChanges();
            }
        }

        private void CancelCollections()
        {
            var cancelChangesCollectionItems = TypeInformationProvider.GetCanCancelChangesSourceCollectionsUnboxed(Type, this);
            foreach (var cancelChangesCollectionItem in cancelChangesCollectionItems.Where(x => x!= null && x.CanCancelChanges))
            {
                cancelChangesCollectionItem.CancelChanges();
            }
        }

        /// <inheritdoc />
        public virtual void MakeDirty()
        {
            if (OwnDirty && CanCancelChanges)
            {
                return;
            }            
            OwnDirty = true;
            AddToHistory();
        }   

        /// <inheritdoc />
        public virtual void ClearDirty(bool forceClearChildren = false)
        {
            OwnDirty = false;            
            if (forceClearChildren)
            {
                var dirtyProperties = TypeInformationProvider.GetDirtySourceValuesUnboxed(Type, this);
                foreach (var dirtyProperty in dirtyProperties)
                {
                    dirtyProperty.ClearDirty(true);
                }
                var dirtyCollectionItems = TypeInformationProvider.GetDirtySourceCollectionsUnboxed(Type, this);
                foreach (var dirtyCollectionItem in dirtyCollectionItems)
                {
                    dirtyCollectionItem.ClearDirty(true);
                }
            }
        }

        private void InitDirtyListener()
        {
            ListenToDirtyPropertyChange();
            var propertyInfos = TypeInformationProvider.GetPropertyDirtySourceCollections(Type, this).ToArray();
            foreach (var propertyInfo in propertyInfos)
            {
                var actualValue = propertyInfo.GetValue(this);
                if (actualValue is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += WeakDelegate.From(NotifyCollectionChangedOnCollectionChanged);
                }
                if (actualValue is IEnumerable<ICanBeDirty> enumerable)
                {
                    foreach (var canBeDirty in enumerable)
                    {
                        NotifyOnInnerChange(canBeDirty);
                    }
                }
            }
        }

        /// <summary>
        /// The internal Dirty Source Collections might change from time to time
        /// In order to keep track of Dirty state changes in their items
        /// we must listen to the <see cref="INotifyCollectionChanged"/> events and subscribe/unsubscribe accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notifyCollectionChangedEventArgs"></param>
        private void NotifyCollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                    case NotifyCollectionChangedAction.Add:
                        var addedItems = notifyCollectionChangedEventArgs.NewItems;
                        foreach (var addedItem in addedItems)
                        {
                            NotifyOnInnerChange(addedItem);
                        }
                        NotifyOfPropertyChange(() => IsDirty);
                        NotifyOfPropertyChange(() => CanCancelChanges);
                    break;
                    case NotifyCollectionChangedAction.Remove:
                        var removedItems = notifyCollectionChangedEventArgs.OldItems;
                        foreach (var removedItem in removedItems)
                        {
                            UnNotifyOnInnerChange(removedItem);
                        }
                        NotifyOfPropertyChange(() => IsDirty);
                        NotifyOfPropertyChange(() => CanCancelChanges);
                    break;
                    case NotifyCollectionChangedAction.Reset:
                        NotifyOfPropertyChange(() => IsDirty);
                        NotifyOfPropertyChange(() => CanCancelChanges);
                    break;
                    case NotifyCollectionChangedAction.Move: case NotifyCollectionChangedAction.Replace:                    
                    break;                    
            }
        }

        private void ListenToDirtyPropertyChange()
        {
            PropertyChanged += WeakDelegate.From(OnDirtyPropertyChanged);
        }

        private void OnDirtyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var changedPropertyName = e.PropertyName;
            
            if (TypeInformationProvider.IsPropertyDirtySource(Type, changedPropertyName) == false)
            {
                return;
            }
            
            var propertyValue = TypeInformationProvider.GetDirtySourceValue(Type, changedPropertyName, this);
            if (propertyValue != null)
            {
                NotifyOnInnerChange(propertyValue);
            }
        }

        /// <summary>
        /// Subscribes to the Dirty state changes of the potential Dirty Source 
        /// </summary>
        /// <param name="notifyingObject"></param>
        private void NotifyOnInnerChange(object notifyingObject)
        {
            _innerChangesSubscriber.SubscribeToNotifyingObjectChanges(notifyingObject, () =>
            {
                //This is the case where an inner Model reports a change in its Dirty state
                //If the current Model is not Dirty yet it should be marked as one
                if (notifyingObject is ICanBeDirty dirtySource && _history.CanUndo == false && dirtySource.IsDirty)
                {
                    AddToHistory();
                }
                NotifyOfPropertyChange(() => IsDirty);
            }, () => NotifyOfPropertyChange(() => CanCancelChanges));            
        }

        /// <summary>
        /// Unsubscribes from the Dirty state changes of the potential Dirty Source 
        /// </summary>
        /// <param name="notifyingObject"></param>
        private void UnNotifyOnInnerChange(object notifyingObject)
        {
            _innerChangesSubscriber.UnsubscribeToNotifyingObjectChanges(notifyingObject);
        }

        /// <inheritdoc />
        public void CommitChanges()
        {
            _checkPoint = new Checkpoint(this);
            ClearDirty(forceClearChildren:true);
        }

        /// <inheritdoc />
        public bool CanCommitChanges => IsDirty;
    }
}
