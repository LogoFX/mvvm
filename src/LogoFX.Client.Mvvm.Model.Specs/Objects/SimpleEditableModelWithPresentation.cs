using System.Collections.Generic;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    class SimpleEditableModelWithPresentation : SimpleEditableModel
    {        
        protected override string CreateErrorsPresentation(IEnumerable<string> errors)
        {
            return "overridden presentation";
        }
    }
}