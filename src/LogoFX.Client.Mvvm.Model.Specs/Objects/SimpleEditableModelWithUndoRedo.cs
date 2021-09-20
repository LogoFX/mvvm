namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    class SimpleEditableModelWithUndoRedo : EditableModel.WithUndoRedo, ISimpleEditableModel
    {        
        public SimpleEditableModelWithUndoRedo(string name, int age)
            : this()
        {
            _name = name;
            _age = age;
        }

        public SimpleEditableModelWithUndoRedo()
        {}

        private string _name;
        [NameValidation]
        public new string Name
        {
            get => _name;
            set
            {
                MakeDirty();
                _name = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => Error);
            }
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age,value);
        }        
    }
}