using LogoFX.Client.Mvvm.Model;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    public interface ISimpleEditableModel : IEditableModel
    {}

    public class SimpleEditableModel : EditableModel, ISimpleEditableModel
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
            get { return _name; }
            set
            {
                var options = new EditableSetPropertyOptions
                {
                    AfterValueUpdate = () => NotifyOfPropertyChange(() => Error)
                };
                SetProperty(ref _name, value, options);
            }
        }
        public int Age { get; set; }
    }
}