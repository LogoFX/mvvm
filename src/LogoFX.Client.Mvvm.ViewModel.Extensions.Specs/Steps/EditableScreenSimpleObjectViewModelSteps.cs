using FluentAssertions;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using LogoFX.Client.Mvvm.ViewModel.Shared;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Steps
{
    [Binding, UsedImplicitly]
    class EditableScreenSimpleObjectViewModelSteps
    {
        private readonly CommonScenarioDataStore<TestConductorViewModel> _commonScenarioDataStore;
        private readonly SimpleScenarioDataStore _simpleScenarioDataStore;

        public EditableScreenSimpleObjectViewModelSteps(
            CommonScenarioDataStore<TestConductorViewModel> commonScenarioDataStore,
            SimpleScenarioDataStore simpleScenarioDataStore)
        {
            _commonScenarioDataStore = commonScenarioDataStore;
            _simpleScenarioDataStore = simpleScenarioDataStore;
        }

        [When(@"I use editable screen simple object view model")]
        public void WhenIUseEditableScreenSimpleObjectViewModel()
        {
            var simpleModel = new SimpleEditableModel();
            _simpleScenarioDataStore.Model = simpleModel;
            var mockMessageService = new FakeMessageService();
            _simpleScenarioDataStore.MockMessageService = mockMessageService;

            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            screenObjectViewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "IsDirty":
                        _simpleScenarioDataStore.WasDirtyRaised = true;
                        break;
                    case "CanCancelChanges":
                        _simpleScenarioDataStore.WasCancelChangesRaised = true;
                        break;
                }
            };
            _simpleScenarioDataStore.SystemUnderTest = screenObjectViewModel;
            _commonScenarioDataStore.RootObject.ActivateItem(screenObjectViewModel);
        }

        [When(@"I set all confirmation to '(.*)'")]
        public void WhenISetAllConfirmationTo(string result)
        {
            switch (result)
            {
                case "Yes":
                    _simpleScenarioDataStore.MockMessageService.SetMessageResult(MessageResult.Yes);
                    break;
                case "No":
                    _simpleScenarioDataStore.MockMessageService.SetMessageResult(MessageResult.No);
                    break;
                case "Cancel":
                    _simpleScenarioDataStore.MockMessageService.SetMessageResult(MessageResult.Cancel);
                    break;
            }
        }

        [When(@"I set the name to be a valid name")]
        public void WhenISetTheNameToBeAValidName()
        {
            _simpleScenarioDataStore.Model.Name = DataGenerator.ValidName;
        }

        [When(@"I close the editable screen object view model")]
        public void WhenICloseTheEditableScreenObjectViewModel()
        {
            _simpleScenarioDataStore.SystemUnderTest.CloseCommand.Execute(null);
        }

        [Then(@"The editable screen object view model is dirty")]
        public void ThenTheEditableScreenObjectViewModelIsDirty()
        {
            _simpleScenarioDataStore.SystemUnderTest.IsDirty.Should().BeTrue();
        }

        [Then(@"The editable screen object view model is not dirty")]
        public void ThenTheEditableScreenObjectViewModelIsNotDirty()
        {
            _simpleScenarioDataStore.SystemUnderTest.IsDirty.Should().BeFalse();
        }

        [Then(@"The model is dirty")]
        public void ThenTheModelIsDirty()
        {
            _simpleScenarioDataStore.Model.IsDirty.Should().BeTrue();
        }

        [Then(@"The model is not dirty")]
        public void ThenTheModelIsNotDirty()
        {
            _simpleScenarioDataStore.Model.IsDirty.Should().BeFalse();
        }

        [Then(@"The changes in the model are saved")]
        public void ThenTheChangesInTheModelAreSaved()
        {
            //TODO: Put intermediate value into the data store
            _simpleScenarioDataStore.Model.Name.Should().Be(DataGenerator.ValidName);
        }

        [Then(@"The changes in the model are not saved")]
        public void ThenTheChangesInTheModelAreNotSaved()
        {
            //TODO: Put intermediate value into the data store
            _simpleScenarioDataStore.Model.Name.Should().NotBe(DataGenerator.ValidName);
        }

        [Then(@"A message is displayed")]
        public void ThenAMessageIsDisplayed()
        {
            _simpleScenarioDataStore.MockMessageService.WasCalled.Should().BeTrue();
        }

        [Then(@"A message is not displayed")]
        public void ThenAMessageIsNotDisplayed()
        {
            _simpleScenarioDataStore.MockMessageService.WasCalled.Should().BeFalse();
        }

        [Then(@"A dirty notification is raised")]
        public void ThenADirtyNotificationIsRaised()
        {
            _simpleScenarioDataStore.WasDirtyRaised.Should().BeTrue();
        }

        [Then(@"A changes cancellation notification is raised")]
        public void ThenAChangesCancellationNotificationIsRaised()
        {
            _simpleScenarioDataStore.WasCancelChangesRaised.Should().BeTrue();
        }
    }
}
