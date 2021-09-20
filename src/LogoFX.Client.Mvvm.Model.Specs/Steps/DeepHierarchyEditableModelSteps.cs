using FluentAssertions;
using LogoFX.Client.Mvvm.Model.Specs.Helpers;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class DeepHierarchyEditableModelSteps
    {
        private readonly CompositeDirtyScenarioDataStore _dirtyScenarioDataStore;
        private readonly ModelSteps _modelSteps;

        public DeepHierarchyEditableModelSteps(
            ScenarioContext scenarioContext, 
            ModelSteps modelSteps)
        {
            _dirtyScenarioDataStore = new CompositeDirtyScenarioDataStore(scenarioContext);
            _modelSteps = modelSteps;
        }

        [When(@"The deep hierarchy model is created")]
        public void WhenTheDeepHierarchyModelIsCreated()
        {
            _modelSteps.CreateModel(() => new DeepHierarchyEditableModel());
        }

        [When(@"The deep hierarchy model with all generations is created")]
        public void WhenTheDeepHierarchyModelWithAllGenerationsIsCreated()
        {
            var grandchild = new SimpleEditableModel(DataGenerator.ValidName, 10);
            var child = new CompositeEditableModel("location", new[] {grandchild});
            _dirtyScenarioDataStore.Child = child;
            _dirtyScenarioDataStore.GrandChild = grandchild;
            _modelSteps.CreateModel(() => { return new DeepHierarchyEditableModel(new[] {child}); });
        }

        [When(@"The child model is created")]
        public void WhenTheChildModelIsCreated()
        {
            var child = new CompositeEditableModel("location");
            _dirtyScenarioDataStore.Child = child;
        }

        [When(@"The first child model is created")]
        public void WhenTheFirstChildModelIsCreated()
        {
            var child = new CompositeEditableModel("location");
            _dirtyScenarioDataStore.FirstChild = child;
        }

        [When(@"The second child model is created")]
        public void WhenTheSecondChildModelIsCreated()
        {
            var child = new CompositeEditableModel("location");
            _dirtyScenarioDataStore.SecondChild = child;
        }

        [When(@"The grandchild model is created")]
        public void WhenTheGrandchildModelIsCreated()
        {
            var grandchild = new SimpleEditableModel(DataGenerator.ValidName, 10);
            _dirtyScenarioDataStore.GrandChild = grandchild;
        }

        [When(@"The first grandchild model is created")]
        public void WhenTheFirstGrandchildModelIsCreated()
        {
            var grandChild = new SimpleEditableModel();
            _dirtyScenarioDataStore.FirstGrandChild = grandChild;
        }

        [When(@"The second grandchild model is created")]
        public void WhenTheSecondGrandchildModelIsCreated()
        {
            var grandChild = new SimpleEditableModel();
            _dirtyScenarioDataStore.SecondGrandChild = grandChild;
        }

        [When(@"The grandchild name is updated to '(.*)'")]
        public void WhenTheGrandchildNameIsUpdatedTo(string name)
        {
            var grandchild = _dirtyScenarioDataStore.GrandChild;
            grandchild.Name = name;
        }

        [When(@"The child is added to the deep hierarchy model")]
        public void WhenTheChildIsAddedToTheDeepHierarchyModel()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            var child = _dirtyScenarioDataStore.Child;
            model.AddCompositeItemImpl(child);
        }

        [When(@"The first child is added to the deep hierarchy model")]
        public void WhenTheFirstChildIsAddedToTheDeepHierarchyModel()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            var child = _dirtyScenarioDataStore.FirstChild;
            model.AddCompositeItemImpl(child);
        }

        [When(@"The second child is added to the deep hierarchy model")]
        public void WhenTheSecondChildIsAddedToTheDeepHierarchyModel()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            var child = _dirtyScenarioDataStore.SecondChild;
            model.AddCompositeItemImpl(child);
        }

        [When(@"The grandchild is added to the child")]
        public void WhenTheGrandchildIsAddedToTheChild()
        {
            var child = _dirtyScenarioDataStore.Child;
            var grandchild = _dirtyScenarioDataStore.GrandChild;
            child.AddSimpleModelImpl(grandchild);
        }

        [When(@"The first grandchild is added to the first child")]
        public void WhenTheFirstGrandchildIsAddedToTheFirstChild()
        {
            var child = _dirtyScenarioDataStore.FirstChild;
            var grandchild = _dirtyScenarioDataStore.FirstGrandChild;
            child.AddSimpleModelImpl(grandchild);
        }

        [When(@"The second grandchild is added to the first child")]
        public void WhenTheSecondGrandchildIsAddedToTheFirstChild()
        {
            var child = _dirtyScenarioDataStore.FirstChild;
            var grandchild = _dirtyScenarioDataStore.SecondGrandChild;
            child.AddSimpleModelImpl(grandchild);
        }

        [When(@"The first child is removed from the deep hierarchy model")]
        public void WhenTheFirstChildIsRemovedFromTheDeepHierarchyModel()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            var child = _dirtyScenarioDataStore.FirstChild;
            model.RemoveCompositeModel(child);
        }

        [When(@"The child is updated with removing the grandchild")]
        public void WhenTheChildIsUpdatedWithRemovingTheGrandchild()
        {
            var child = _dirtyScenarioDataStore.Child;
            var grandchild = _dirtyScenarioDataStore.GrandChild;
            child.RemoveSimpleItem(grandchild);
        }

        [When(@"The first child is updated with removing the first grandchild")]
        public void WhenTheFirstChildIsUpdatedWithRemovingTheFirstGrandchild()
        {
            var child = _dirtyScenarioDataStore.FirstChild;
            var grandchild = _dirtyScenarioDataStore.FirstGrandChild;
            child.RemoveSimpleItem(grandchild);
        }

        [When(@"The deep hierarchy model changes are committed")]
        public void WhenTheDeepHierarchyModelChangesAreCommitted()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            model.CommitChanges();
        }

        [When(@"The deep hierarchy model changes are cancelled")]
        public void WhenTheDeepHierarchyModelChangesAreCancelled()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            model.CancelChanges();
        }

        [Then(@"The deep hierarchy model is not marked as dirty")]
        public void ThenTheDeepHierarchyModelIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            model.IsDirty.Should().BeFalse();
        }

        [Then(@"The grandchild name should be identical to the valid name")]
        public void ThenTheGrandchildNameShouldBeIdenticalToTheValidName()
        {
            var grandchild = _dirtyScenarioDataStore.GrandChild;
            grandchild.Name.Should().Be(DataGenerator.ValidName);
        }

        [Then(@"The deep hierarchy model changes can be cancelled")]
        public void ThenTheDeepHierarchyModelChangesCanBeCancelled()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            model.CanCancelChanges.Should().BeTrue();
        }

        [Then(@"The deep hierarchy model changes can not be cancelled")]
        public void ThenTheDeepHierarchyModelChangesCanNotBeCancelled()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            model.CanCancelChanges.Should().BeFalse();
        }

        [Then(@"The deep hierarchy model contains the child")]
        public void ThenTheDeepHierarchyModelContainsTheChild()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            var child = _dirtyScenarioDataStore.Child;
            model.CompositeModels.Should().BeEquivalentTo(new[] {child});
        }

        [Then(@"The deep hierarchy model contains all children")]
        public void ThenTheDeepHierarchyModelContainsAllChildren()
        {
            var model = _modelSteps.GetModel<DeepHierarchyEditableModel>();
            var children = new[]
            {
                _dirtyScenarioDataStore.FirstChild,
                _dirtyScenarioDataStore.SecondChild
            };
            model.CompositeModels.Should().BeEquivalentTo(children);
        }

        [Then(@"The child contains the grandchild")]
        public void ThenTheChildContainsTheGrandchild()
        {
            var grandchild = _dirtyScenarioDataStore.GrandChild;
            var child = _dirtyScenarioDataStore.Child;
            child.SimpleCollection.Should().BeEquivalentTo(new[] {grandchild});
        }

        [Then(@"The child does not contain the grandchild")]
        public void ThenTheChildDoesNotContainTheGrandchild()
        {
            var grandchild = _dirtyScenarioDataStore.GrandChild;
            var child = _dirtyScenarioDataStore.Child;
            child.SimpleCollection.Should().NotContain(grandchild);
        }

        [Then(@"The first child contains all grandchildren")]
        public void ThenTheFirstChildContainsAllGrandchildren()
        {
            var grandchildren = new[]
            {
                _dirtyScenarioDataStore.FirstGrandChild,
                _dirtyScenarioDataStore.SecondGrandChild
            };
            var child = _dirtyScenarioDataStore.FirstChild;
            child.SimpleCollection.Should().BeEquivalentTo(grandchildren);
        }
    }
}
