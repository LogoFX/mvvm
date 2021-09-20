using System.Threading.Tasks;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    class TestEditableObjectViewModel : EditableObjectViewModel<SimpleEditableModel>
    {
        public TestEditableObjectViewModel(SimpleEditableModel model) : base(model)
        {
        }

        protected override Task<bool> SaveMethod(SimpleEditableModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}