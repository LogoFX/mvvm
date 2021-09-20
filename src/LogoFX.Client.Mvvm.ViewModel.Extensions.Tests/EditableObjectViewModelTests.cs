using FluentAssertions;
using Xunit;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Tests
{
    public class EditableObjectViewModelTests
    {
        [Fact]
        public void ModelIsChanged_ViewModelRaisesNotifications()
        {
            var simpleModel = new SimpleEditableModel();          

            var rootObject = new TestEditableObjectViewModel(simpleModel);            
            bool wasDirtyRaised = false, wasCancelChangesRaised = false;

            rootObject.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "IsDirty":
                        wasDirtyRaised = true;
                        break;
                    case "CanCancelChanges":
                        wasCancelChangesRaised = true;
                        break;
                }
            };            
            simpleModel.Name = DataGenerator.ValidName;

            wasDirtyRaised.Should().BeTrue();
            wasCancelChangesRaised.Should().BeTrue();
        }
    }
}
