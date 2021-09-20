using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    class ExplicitCompositeEditableModel : EditableModel, ICompositeEditableModel
    {
        public ExplicitCompositeEditableModel(string location)
        {
            Location = location;
            _person = new SimpleEditableModel();
        }

        public ExplicitCompositeEditableModel(string location, IEnumerable<int> phones)
        {
            Location = location;
            _person = new SimpleEditableModel();
            Phones.AddRange(phones);
        }

        public ExplicitCompositeEditableModel(string location, IEnumerable<SimpleEditableModel> simpleCollection)
        {
            Location = location;
            _person = new SimpleEditableModel();
            foreach (var simpleEditableModel in simpleCollection)
            {
                _simpleCollection.Add(simpleEditableModel);
            }
        }

        public string Location { get; }

        private ISimpleEditableModel _person;
        public ISimpleEditableModel Person
        {
            get => _person;
            set => SetProperty(ref _person, value, new EditableSetPropertyOptions()
            {
                MarkAsDirty = false
            });
        }

        private readonly ObservableCollection<SimpleEditableModel> _simpleCollection = new ObservableCollection<SimpleEditableModel>();

        IEnumerable<ISimpleEditableModel> ICompositeEditableModel.SimpleCollection => _simpleCollection;

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
            _simpleCollection.Remove(item);
        }
    }
}