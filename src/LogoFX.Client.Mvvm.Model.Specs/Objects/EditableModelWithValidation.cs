namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    internal sealed class EditableModelWithValidation : EditableModel
    {
        private string _title;

        public EditableModelWithValidation(string title, int age)
        {
            _title = title;
            Age = age;
        }

        [TitleValidation]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public int Age { get; set; }
    }
}