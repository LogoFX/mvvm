using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    public abstract class VirtualContainerViewModel<TModel, TViewModel> : Conductor<TViewModel>, IModelWrapper<TModel>, IHaveSubViewModel
        where TViewModel : class, IModelWrapper<TModel>
    {
        private bool _isSubViewModelVisible;
        private TViewModel _subViewModel;

        protected VirtualContainerViewModel(TModel model)
        {
            Model = model;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(ref _isSelected, value); }
        }

        public TViewModel SubViewModel
        {
            get { return GetSubViewModel(); }
        }
        
        protected abstract TViewModel CreateSubViewModel();

        private TViewModel GetSubViewModel()
        {
            return _subViewModel ?? (_subViewModel = CreateSubViewModel());
        }

        private void UpdateSubViewModel()
        {
            ActivateItem(IsSubViewModelVisible ? GetSubViewModel() : null);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            if (IsSubViewModelVisible)
            {
                UpdateSubViewModel();
            }
        }

        object IModelWrapper.Model => Model;

        public TModel Model { get; private set; }

        public bool IsSubViewModelVisible
        {
            get => _isSubViewModelVisible;
            set
            {
                if (Set(ref _isSubViewModelVisible, value))
                {
                    UpdateSubViewModel();
                }
            }
        }
    }
}