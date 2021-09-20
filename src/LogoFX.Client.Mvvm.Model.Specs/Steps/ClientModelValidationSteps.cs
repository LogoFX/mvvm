using FluentAssertions;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class ClientModelValidationSteps
    {
        private readonly SimpleValidationScenarioDataStore _simpleValidationScenarioDataStore;

        public ClientModelValidationSteps(ScenarioContext scenarioContext)
        {
            _simpleValidationScenarioDataStore = new SimpleValidationScenarioDataStore(scenarioContext);
        }

        [When(@"The simple test value object is created with name '(.*)'")]
        public void WhenTheSimpleTestValueObjectIsCreatedWithName(string name)
        {
            var valueObject = new SimpleTestValueObject(name, 5);
            _simpleValidationScenarioDataStore.ValueObject = valueObject;
        }

        [Then(@"The simple test value object has no errors")]
        public void ThenTheSimpleTestValueObjectHasNoErrors()
        {
            var valueObject = _simpleValidationScenarioDataStore.ValueObject;
            var error = valueObject.Error;
            error.Should().BeNullOrEmpty();
        }

        [Then(@"The simple test value object has errors")]
        public void ThenTheSimpleTestValueObjectHasErrors()
        {
            var valueObject = _simpleValidationScenarioDataStore.ValueObject;
            var error = valueObject.Error;
            error.Should().NotBeNullOrEmpty();
        }
    }
}
