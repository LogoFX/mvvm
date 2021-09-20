using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Steps
{
    [Binding, UsedImplicitly]
    class EditableScreenCompositeObjectViewModelSteps
    {
        private readonly CommonScenarioDataStore<TestConductorViewModel> _commonScenarioDataStore;
        private readonly CompositeScenarioDataStore _compositeScenarioDataStore;

        public EditableScreenCompositeObjectViewModelSteps(
            CommonScenarioDataStore<TestConductorViewModel> commonScenarioDataStore,
            CompositeScenarioDataStore compositeScenarioDataStore)
        {
            _commonScenarioDataStore = commonScenarioDataStore;
            _compositeScenarioDataStore = compositeScenarioDataStore;
        }

        [When(@"I use editable screen composite object view model")]
        public void WhenIUseEditableScreenCompositeObjectViewModel()
        {
            var initialPhones = new[] { 546, 432 };
            var compositeModel = new CompositeEditableModel("Here", initialPhones);
            _compositeScenarioDataStore.Model = compositeModel;
            var mockMessageService = new FakeMessageService();
            _compositeScenarioDataStore.MockMessageService = mockMessageService;

            var screenObjectViewModel = new TestEditableScreenCompositeObjectViewModel(mockMessageService, compositeModel);
            _compositeScenarioDataStore.SystemUnderTest = screenObjectViewModel;
            _commonScenarioDataStore.RootObject.ActivateItem(screenObjectViewModel);
        }

        [When(@"I add phone (.*)")]
        public void WhenIAddPhone(int phone)
        {
            _compositeScenarioDataStore.Model.AddPhone(phone);
        }

        [When(@"I apply the changes")]
        public void WhenIApplyTheChanges()
        {
            _compositeScenarioDataStore.SystemUnderTest.ApplyCommand.Execute(null);
        }

        [When(@"I cancel the changes")]
        public void WhenICancelTheChanges()
        {
            _compositeScenarioDataStore.SystemUnderTest.CancelChangesCommand.Execute(null);
        }

        [Then(@"The model should contain correct phones")]
        public void ThenTheModelShouldContainCorrectPhones()
        {
            var phones = ((ICompositeEditableModel)_compositeScenarioDataStore.Model).Phones.ToArray();
            //TODO: Extract the value from data store
            var expectedPhones = new[] { 546, 432, 645 };
            phones.Should().BeEquivalentTo(expectedPhones);
        }
    }
}