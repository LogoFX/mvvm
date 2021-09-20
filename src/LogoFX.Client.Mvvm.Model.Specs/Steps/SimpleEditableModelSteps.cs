using System.Linq;
using FluentAssertions;
using LogoFX.Client.Mvvm.Model.Specs.Helpers;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class SimpleEditableModelSteps
    {
        private readonly SimpleEditableScenarioDataStore _scenarioDataStore;
        private readonly ModelSteps _modelSteps;
        private readonly ValidationSteps _validationSteps;

        public SimpleEditableModelSteps(
            ScenarioContext scenarioContext,
            ModelSteps modelSteps,
            ValidationSteps validationSteps)
        {
            _scenarioDataStore = new SimpleEditableScenarioDataStore(scenarioContext);
            _modelSteps = modelSteps;
            _validationSteps = validationSteps;
        }

        [When(@"The simple editable model is created with valid name")]
        public void WhenTheSimpleEditableModelIsCreatedWithValidName()
        {
            CreateSimpleEditableModel(DataGenerator.ValidName);
        }

        [When(@"The simple editable model is created with invalid name")]
        public void WhenTheSimpleEditableModelIsCreatedWithInvalidName()
        {
            CreateSimpleEditableModel(DataGenerator.InvalidName);
        }

        private void CreateSimpleEditableModel(string name)
        {
            _modelSteps.CreateModel(() =>
                new SimpleEditableModel(name, 5));
        }

        [When(@"The simple editable model with overridden presentation is created")]
        public void WhenTheSimpleEditableModelWithOverriddenPresentationIsCreated()
        {
            _modelSteps.CreateModel(() => 
                new SimpleEditableModelWithPresentation());
        }

        [When(@"The simple editable model is updated with external error")]
        public void WhenTheSimpleEditableModelIsUpdatedWithExternalError()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            var propertyName = "Name";
            _scenarioDataStore.PropertyName = propertyName;
            model.SetError("external error", "Name");
        }

        [When(@"The simple editable model is cleared from external errors")]
        public void WhenTheSimpleEditableModelIsClearedFromExternalErrors()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            var propertyName = _scenarioDataStore.PropertyName;
            model.ClearError(propertyName);
        }

        [When(@"The simple editable model is updated with invalid value for property")]
        public void WhenTheSimpleEditableModelIsUpdatedWithInvalidValueForProperty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            model.Name = DataGenerator.InvalidName;
        }

        [Then(@"The simple editable model has no errors")]
        public void ThenTheSimpleEditableModelHasNoErrors()
        {
            _validationSteps.AssertModelHasNoError(_modelSteps.GetModel<SimpleEditableModel>);
        }

        [Then(@"The simple editable model has errors")]
        public void ThenTheSimpleEditableModelHasErrors()
        {
            _validationSteps.AssertModelHasError(_modelSteps.GetModel<SimpleEditableModel>);
        }

        [Then(@"The errors collection for property without validation info is empty")]
        public void ThenTheErrorsCollectionForPropertyWithoutValidationInfoIsEmpty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            var errors = model.GetErrors(nameof(model.Age));
            errors.OfType<object>().Should().BeEmpty();
        }

        [Then(@"The simple editable model with presentation error should be '(.*)'")]
        public void ThenTheSimpleEditableModelWithPresentationErrorShouldBe(string expectedError)
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            var error = model.Error;
            error.Should().Be(expectedError);
        }
    }
}
