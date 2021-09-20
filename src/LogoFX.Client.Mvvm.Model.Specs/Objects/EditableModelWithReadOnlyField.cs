using System;

namespace LogoFX.Client.Mvvm.Model.Specs.Objects
{
    public class EditableModelWithReadOnlyField : EditableModel<Guid>
    {
        public EditableModelWithReadOnlyField(int status, string logRemark)
        {
            Status = status;
            LogRemark = logRemark;
        }

        private int _status;
        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string LogRemark { get; }
    }
}