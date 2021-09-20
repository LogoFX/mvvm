using LogoFX.Client.Mvvm.Model.Specs.Helpers;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class CompositeEditableModelSteps
    {
        private readonly SimpleDirtyScenarioDataStore _simpleDirtyScenarioDataStore;
        private readonly ModelSteps _modelSteps;
        private readonly ValidationSteps _validationSteps;

        public CompositeEditableModelSteps(
            ScenarioContext scenarioContext,
            ModelSteps modelSteps,
            ValidationSteps validationSteps)
        {
            _simpleDirtyScenarioDataStore = new SimpleDirtyScenarioDataStore(scenarioContext);
            _modelSteps = modelSteps;
            _validationSteps = validationSteps;
        }

        [When(@"The composite editable model is created")]
        public void WhenTheCompositeEditableModelIsCreated()
        {
            _modelSteps.CreateModel(() => 
                new CompositeEditableModel("location"));
        }

        [When(@"The composite editable model with collection is created")]
        public void WhenTheCompositeEditableModelWithCollectionIsCreated()
        {
            var child = new SimpleEditableModel();
            _simpleDirtyScenarioDataStore.Child = child;
            _modelSteps.CreateModel(() =>
                new CompositeEditableModel("location",
                    new[] {child}));
        }

        [When(@"The explicit composite editable model with collection is created")]
        public void WhenTheExplicitCompositeEditableModelWithCollectionIsCreated()
        {
            var child = new SimpleEditableModel();
            _simpleDirtyScenarioDataStore.Child = child;
            _modelSteps.CreateModel(() =>
                new ExplicitCompositeEditableModel("location",
                    new[] { child }));
        }

        [When(@"The internal model property is assigned a valid value")]
        public void WhenTheInternalModelPropertyIsAssignedAValidValue()
        {
            var compositeModel = _modelSteps.GetModel<CompositeEditableModel>();
            compositeModel.Person.Name = DataGenerator.ValidName;
        }

        [When(@"The internal model property is assigned an invalid value")]
        public void WhenTheInternalModelPropertyIsAssignedAnInvalidValue()
        {
            var compositeModel = _modelSteps.GetModel<CompositeEditableModel>();
            compositeModel.Person.Name = DataGenerator.InvalidName;
        }

        [When(@"The internal model is reset")]
        public void WhenTheInternalModelIsReset()
        {
            var compositeModel = _modelSteps.GetModel<CompositeEditableModel>();
            compositeModel.Person = new SimpleEditableModel(DataGenerator.ValidName, 0);
        }

        [Then(@"The composite editable model has no errors")]
        public void ThenTheCompositeEditableModelHasNoErrors()
        {
            _validationSteps.AssertModelHasNoError(_modelSteps.GetModel<CompositeEditableModel>);
        }

        [Then(@"The composite editable model has errors")]
        public void ThenTheCompositeEditableModelHasErrors()
        {
            _validationSteps.AssertModelHasError(_modelSteps.GetModel<CompositeEditableModel>);
        }

        [Then(@"The error notification should be raised")]
        public void ThenTheErrorNotificationShouldBeRaised()
        {
            _modelSteps.AssertNotificationIsRaised(NotificationKind.Error);
        }
    }
}
