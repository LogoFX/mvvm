using LogoFX.Client.Mvvm.Model.Specs.Objects;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class SelfReferencingModelSteps
    {
        private readonly ModelSteps _modelSteps;

        public SelfReferencingModelSteps(
            ModelSteps modelSteps)
        {
            _modelSteps = modelSteps;
        }

        [When(@"The self-referencing model is created")]
        public void WhenTheSelf_ReferencingModelIsCreated()
        {
            _modelSteps.CreateModel(() => new SelfEditableModel());
        }

        [When(@"The self-referencing model is assigned itself")]
        public void WhenTheSelf_ReferencingModelIsAssignedItself()
        {
            var model = _modelSteps.GetModel<SelfEditableModel>();
            model.Self = model;
        }
    }
}
