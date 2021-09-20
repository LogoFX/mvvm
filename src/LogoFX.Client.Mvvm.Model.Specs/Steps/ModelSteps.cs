using System;
using System.ComponentModel;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.Model.Specs.Steps
{
    [Binding]
    internal sealed class ModelSteps
    {
        private readonly ModelScenarioDataStore _scenarioDataStore;

        public ModelSteps(ScenarioContext scenarioContext)
        {
            _scenarioDataStore = new ModelScenarioDataStore(scenarioContext);
        }

        internal void CreateModel<T>(Func<T> modelFactory)
            where T : INotifyPropertyChanged
        {
            var model = modelFactory();
            var isErrorRaised = false;
            var isErrorRaisedRef = new WeakReference(isErrorRaised);
            var isDirtyRaised = false;
            var isDirtyRaisedRef = new WeakReference(isDirtyRaised);
            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Error")
                {
                    isErrorRaisedRef.Target = true;
                }

                if (args.PropertyName == "IsDirty")
                {
                    isDirtyRaisedRef.Target = true;
                }
            };
            _scenarioDataStore.Model = model;
            _scenarioDataStore.IsDirtyRaisedRef = isDirtyRaisedRef;
            _scenarioDataStore.IsErrorRaisedRef = isErrorRaisedRef;
        }

        internal T GetModel<T>() where T : class =>
            _scenarioDataStore.Model as T;

        internal void AssertNotificationIsRaised(NotificationKind kind)
        {
            Func<ModelScenarioDataStore, WeakReference> valueGetter = null;
            switch (kind)
            {
                case NotificationKind.Dirty:
                    valueGetter = r => r.IsDirtyRaisedRef;
                    break;
                case NotificationKind.Error:
                    valueGetter = r => r.IsErrorRaisedRef;
                    break;
            }
            if (valueGetter == null)
            {
                throw new NotSupportedException($"Notification {kind} is not supported");
            }
            var weakReference = valueGetter(_scenarioDataStore);
            ((bool)weakReference.Target).Should().BeTrue();
        }
    }
}
