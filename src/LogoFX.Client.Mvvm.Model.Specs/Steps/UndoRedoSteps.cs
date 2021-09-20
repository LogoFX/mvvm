using System.Linq;
using FluentAssertions;
using LogoFX.Client.Mvvm.Model.Specs.Helpers;
using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class UndoRedoSteps
    {
        private readonly UndoRedoScenarioDataStore _scenarioDataStore;
        private readonly ModelSteps _modelSteps;

        public UndoRedoSteps(
            ScenarioContext scenarioContext,
            ModelSteps modelSteps)
        {
            _scenarioDataStore = new UndoRedoScenarioDataStore(scenarioContext);
            _modelSteps = modelSteps;
        }

        [When(@"The simple editable model with undo-redo is created with valid name")]
        public void WhenTheSimpleEditableModelWithUndo_RedoIsCreatedWithValidName()
        {
            _modelSteps.CreateModel(() =>
                new SimpleEditableModelWithUndoRedo(DataGenerator.ValidName, 5));
        }

        [When(@"The composite editable model with undo-redo is created with initial data")]
        public void WhenTheCompositeEditableModelWithUndo_RedoIsCreatedWithInitialData()
        {
            var data = new[] {546, 432};
            _modelSteps.CreateModel(() =>
                new CompositeEditableModelWithUndoRedo("Here", data));
            _scenarioDataStore.Data = data;
        }

        [When(@"The composite editable model with undo-redo is created with inner model")]
        public void WhenTheCompositeEditableModelWithUndo_RedoIsCreatedWithInnerModel()
        {
            var child = new SimpleEditableModel(DataGenerator.ValidName, 25);
            _scenarioDataStore.Child = child;
            _modelSteps.CreateModel(() => new CompositeEditableModelWithUndoRedo("Here", new[] {child}));
        }

        [When(@"The name is updated to '(.*)'")]
        public void WhenTheNameIsUpdatedTo(string name)
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.Name = name;
        }

        [When(@"The collection of items is updated with new value (.*)")]
        public void WhenTheCollectionOfItemsIsUpdatedWithNewValue(int value)
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.AddPhone(value);
            if (_scenarioDataStore.Value == default)
            {
                _scenarioDataStore.Value = value;
            }
        }

        [When(@"The inner model property is updated with the new value '(.*)'")]
        public void WhenTheInnerModelPropertyIsUpdatedWithTheNewValue(string name)
        {
            var child = _scenarioDataStore.Child;
            child.Name = name;
        }

        [When(@"The last operation for simple editable model with undo-redo is undone")]
        public void WhenTheLastOperationForSimpleEditableModelWithUndo_RedoIsUndone()
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.Undo();
        }

        [When(@"The last operation for simple editable model with undo-redo is redone")]
        public void WhenTheLastOperationForSimpleEditableModelWithUndo_RedoIsRedone()
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.Redo();
        }

        [When(@"The last operation for composite editable model with undo-redo is undone")]
        public void WhenTheLastOperationForCompositeEditableModelWithUndo_RedoIsUndone()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.Undo();
        }

        [When(@"The last operation for composite editable model with undo-redo is redone")]
        public void WhenTheLastOperationForCompositeEditableModelWithUndo_RedoIsRedone()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.Redo();
        }

        [Then(@"The name should be '(.*)'")]
        public void ThenTheNameShouldBe(string expectedName)
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.Name.Should().Be(expectedName);
        }

        [Then(@"The name should be identical to the valid name")]
        public void ThenTheNameShouldBeIdenticalToTheValidName()
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.Name.Should().Be(DataGenerator.ValidName);
        }

        [Then(@"The collection of items should be equivalent to the initial data")]
        public void ThenTheCollectionOfItemsShouldBeEquivalentToTheInitialData()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            var data = _scenarioDataStore.Data;
            ((ICompositeEditableModel) model).Phones.Should().BeEquivalentTo(data);
        }

        [Then(@"The collection of items should be equivalent to the initial data with the new value")]
        public void ThenTheCollectionOfItemsShouldBeEquivalentToTheInitialDataWithTheNewValue()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            var data = _scenarioDataStore.Data;
            var value = _scenarioDataStore.Value;
            ((ICompositeEditableModel) model).Phones.Should().BeEquivalentTo(data.Append(value));
        }

        [Then(@"The composite editable model with undo-redo can be undone")]
        public void ThenTheCompositeEditableModelWithUndo_RedoCanBeUndone()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.CanUndo.Should().BeTrue();
        }

        [Then(@"The simple editable model with undo-redo is marked as dirty")]
        public void ThenTheSimpleEditableModelWithUndo_RedoIsMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.IsDirty.Should().BeTrue();
        }

        [Then(@"The simple editable model with undo-redo is not marked as dirty")]
        public void ThenTheSimpleEditableModelWithUndo_RedoIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.IsDirty.Should().BeFalse();
        }

        [Then(@"The composite editable model with undo-redo is marked as dirty")]
        public void ThenTheCompositeEditableModelWithUndo_RedoIsMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.IsDirty.Should().BeTrue();
        }

        [Then(@"The composite editable model with undo-redo is not marked as dirty")]
        public void ThenTheCompositeEditableModelWithUndo_RedoIsNotMarkedAsDirty()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.IsDirty.Should().BeFalse();
        }
    }
}