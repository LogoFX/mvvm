using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class EditingLifecycleSteps
    {
        private readonly ModelSteps _modelSteps;

        public EditingLifecycleSteps(
            ModelSteps modelSteps)
        {
            _modelSteps = modelSteps;
        }

        [When(@"The editable model changes are cancelled")]
        public void WhenTheEditableModelChangesAreCancelled()
        {
            var model =_modelSteps.GetModel<EditableModelWithValidation>();
            model.CancelChanges();
        }

        [When(@"The editable model with read only field changes are cancelled")]
        public void WhenTheEditableModelWithReadOnlyFieldChangesAreCancelled()
        {
            var model = _modelSteps.GetModel<EditableModelWithReadOnlyField>();
            model.CancelChanges();
        }

        [When(@"The editable model with undo redo changes are cancelled")]
        public void WhenTheEditableModelWithUndoRedoChangesAreCancelled()
        {
            var model = _modelSteps.GetModel<SimpleEditableModelWithUndoRedo>();
            model.CancelChanges();
        }

        [When(@"The composite editable model with undo-redo changes are cancelled")]
        public void WhenTheCompositeEditableModelWithUndo_RedoChangesAreCancelled()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.CancelChanges();
        }

        [When(@"The self-referencing model changes are cancelled")]
        public void WhenTheSelf_ReferencingModelChangesAreCancelled()
        {
            var model = _modelSteps.GetModel<SelfEditableModel>();
            model.CancelChanges();
        }

        [When(@"The composite editable model with undo-redo changes are committed")]
        public void WhenTheCompositeEditableModelWithUndo_RedoChangesAreCommitted()
        {
            var model = _modelSteps.GetModel<CompositeEditableModelWithUndoRedo>();
            model.CommitChanges();
        }
    }
}
