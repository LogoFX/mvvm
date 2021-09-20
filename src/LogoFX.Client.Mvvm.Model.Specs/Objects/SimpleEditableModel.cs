using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    interface ISimpleEditableModel : IEditableModel, ISimpleModel
    {}        

    class SimpleEditableModel : EditableModel, ISimpleEditableModel
    {
        public SimpleEditableModel(string name, int age)
            : this()
        {
            _name = name;
            Age = age;
        }

        public SimpleEditableModel()
        {}

        private string _name;
        [NameValidation]
        public new string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public int Age { get; set; }        
    }
}