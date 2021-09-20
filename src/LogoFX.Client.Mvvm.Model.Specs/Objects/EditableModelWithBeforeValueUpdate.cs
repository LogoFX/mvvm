using System;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    public class EditableModelWithBeforeValueUpdate : EditableModel<Guid>
    {
        public EditableModelWithBeforeValueUpdate(int status)
        {
            Status = status;
        }

        private int _status;
        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value, new EditableSetPropertyOptions()
            {
                BeforeValueUpdate = () => PreviousValue = _status
            });
        }

        public int PreviousValue { get; private set; }
    }
}