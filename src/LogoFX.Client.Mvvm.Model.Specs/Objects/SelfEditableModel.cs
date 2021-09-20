namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    public class SelfEditableModel : EditableModel
    {
        private string _value;

        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public SelfEditableModel Self { get; set; }
    }
}