using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    class CompositeEditableModelWithUndoRedo : EditableModel.WithUndoRedo, ICompositeEditableModel, ICloneable<CompositeEditableModelWithUndoRedo>, IEquatable<CompositeEditableModelWithUndoRedo>
    {
        public CompositeEditableModelWithUndoRedo(string location)
        {
            Location = location;
            _person = new SimpleEditableModel();
        }

        public CompositeEditableModelWithUndoRedo(string location, IEnumerable<int> phones)
        {
            Location = location;
            _person = new SimpleEditableModel();
            Phones.AddRange(phones);
        }

        public CompositeEditableModelWithUndoRedo(string location, IEnumerable<SimpleEditableModel> simpleCollection)
        {
            Location = location;
            _person = new SimpleEditableModel();
            foreach (var simpleEditableModel in simpleCollection)
            {
                SimpleCollectionImpl.Add(simpleEditableModel);
            }
        }

        public string Location { get; }

        private ISimpleEditableModel _person;
        public ISimpleEditableModel Person
        {
            get => _person;
            set => SetProperty(ref _person, value);
        }

        private ObservableCollection<SimpleEditableModel> SimpleCollectionImpl { get; } = new ObservableCollection<SimpleEditableModel>();

        public IEnumerable<ISimpleEditableModel> SimpleCollection => SimpleCollectionImpl;

        IEnumerable<int> ICompositeEditableModel.Phones => Phones;

        private List<int> Phones { get; } = new List<int>();

        public void AddPhone(int number)
        {
            MakeDirty();
            Phones.Add(number);
        }

        public void RemoveSimpleItem(SimpleEditableModel item)
        {
            MakeDirty();
            SimpleCollectionImpl.Remove(item);
        }

        public void AddSimpleModelImpl(SimpleEditableModel simpleEditableModel)
        {
            SimpleCollectionImpl.Add(simpleEditableModel);
        }

        public CompositeEditableModelWithUndoRedo Clone()
        {
            var composite = new CompositeEditableModelWithUndoRedo(Location, Phones);
            composite.Id = composite.Id;
            foreach (var simpleEditableModel in SimpleCollectionImpl)
            {
                composite.AddSimpleModelImpl(simpleEditableModel);
            }
            return composite;
        }

        public bool Equals(CompositeEditableModelWithUndoRedo other)
        {
            return other != null && other.Id == Id;
        }
    }
}