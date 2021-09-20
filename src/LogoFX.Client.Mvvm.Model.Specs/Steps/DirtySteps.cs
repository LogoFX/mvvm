using FluentAssertions;
using LogoFX.Client.Mvvm.Model.Specs.Helpers;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class DirtySteps
    {
        private readonly SimpleDirtyScenarioDataStore _dirtyScenarioDataStore;
        private readonly ModelSteps _modelSteps;

        public DirtySteps(
            ScenarioContext scenarioContext,
            ModelSteps modelSteps)
        {
            _dirtyScenarioDataStore = new SimpleDirtyScenarioDataStore(scenarioContext);
            _modelSteps = modelSteps;
        }

        [When(@"The simple editable model is made dirty")]
        public void WhenTheSimpleEditableModelIsMadeDirty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            model.MakeDirty();
        }

        [When(@"The composite editable model is updated with invalid value for inner property value")]
        public void WhenTheCompositeEditableModelIsUpdatedWithInvalidValueForInnerPropertyValue()
        {
            var model = _modelSteps.GetModel<CompositeEditableModel>();
            model.Person.Name = DataGenerator.InvalidName;
        }

        [When(@"The composite editable model is updated with invalid value for child collection item inner property value")]
        public void WhenTheCompositeEditableModelIsUpdatedWithInvalidValueForChildCollectionItemInnerPropertyValue()
        {
            var child = _dirtyScenarioDataStore.Child;
            child.Name = DataGenerator.InvalidName;
        }

        [When(@"The composite editable model is updated by removing child item from the collection")]
        public void WhenTheCompositeEditableModelIsUpdatedByRemovingChildItemFromTheCollection()
        {
            var model = _modelSteps.GetModel<CompositeEditableModel>();
            var child = _dirtyScenarioDataStore.Child;
            model.RemoveSimpleItem(child);
        }

        [When(@"The explicit composite editable model is updated by removing child item from the collection")]
        public void WhenTheExplicitCompositeEditableModelIsUpdatedByRemovingChildItemFromTheCollection()
        {
            var model = _modelSteps.GetModel<ExplicitCompositeEditableModel>();
            var child = _dirtyScenarioDataStore.Child;
            model.RemoveSimpleItem(child);
        }

        [When(@"The composite editable model is cleared of dirty state along with its children")]
        public void WhenTheCompositeEditableModelIsClearedOfDirtyStateAlongWithItsChildren()
        {
            var model = _modelSteps.GetModel<CompositeEditableModel>();
            model.ClearDirty(forceClearChildren:true);
        }

        [When(@"The composite editable model is cleared of dirty state without its children")]
        public void WhenTheCompositeEditableModelIsClearedOfDirtyStateWithoutItsChildren()
        {
            var model = _modelSteps.GetModel<CompositeEditableModel>();
            model.ClearDirty(forceClearChildren: false);
        }

        [When(@"The explicit composite editable model is cleared of dirty state along with its children")]
        public void WhenTheExplicitCompositeEditableModelIsClearedOfDirtyStateAlongWithItsChildren()
        {
            var model = _modelSteps.GetModel<ExplicitCompositeEditableModel>();
            model.ClearDirty(forceClearChildren: true);
        }

        [When(@"The child item is assigned an invalid property value")]
        public void WhenTheChildItemIsAssignedAnInvalidPropertyValue()
        {
            var child = _dirtyScenarioDataStore.Child;
            child.Name = DataGenerator.InvalidName;
        }

        [Then(@"The simple editable model is not marked as dirty")]
        public void ThenTheSimpleEditableModelIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            model.IsDirty.Should().BeFalse();
        }

        [Then(@"The simple editable model is marked as dirty")]
        public void ThenTheSimpleEditableModelIsMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModel>();
            model.IsDirty.Should().BeTrue();
        }

        [Then(@"The composite editable model is not marked as dirty")]
        public void ThenTheCompositeEditableModelIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<CompositeEditableModel>();
            model.IsDirty.Should().BeFalse();
        }

        [Then(@"The composite editable model is marked as dirty")]
        public void ThenTheCompositeEditableModelIsMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<CompositeEditableModel>();
            model.IsDirty.Should().BeTrue();
        }

        [Then(@"The explicit composite editable model is marked as dirty")]
        public void ThenTheExplicitCompositeEditableModelIsMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<ExplicitCompositeEditableModel>();
            model.IsDirty.Should().BeTrue();
        }

        [Then(@"The explicit composite editable model is not marked as dirty")]
        public void ThenTheExplicitCompositeEditableModelIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<ExplicitCompositeEditableModel>();
            model.IsDirty.Should().BeFalse();
        }

        [Then(@"The self-referencing model is marked as dirty")]
        public void ThenTheSelf_ReferencingModelIsMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<SelfEditableModel>();
            model.IsDirty.Should().BeTrue();
        }

        [Then(@"The self-referencing model is not marked as dirty")]
        public void ThenTheSelf_ReferencingModelIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<SelfEditableModel>();
            model.IsDirty.Should().BeFalse();
        }

        [Then(@"The dirty notification should be raised")]
        public void ThenTheDirtyNotificationShouldBeRaised()
        {
            _modelSteps.AssertNotificationIsRaised(NotificationKind.Dirty);
        }
    }
}
