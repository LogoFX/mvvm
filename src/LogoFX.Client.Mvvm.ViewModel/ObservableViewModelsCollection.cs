using System.Collections.ObjectModel;
using LogoFX.Client.Mvvm.ViewModel.Contracts;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// ObservableViewModelsCollection
    /// </summary>
    public class ObservableViewModelsCollection<T> : ObservableCollection<T>, IObjectViewModelsCollection<T> where T : IObjectViewModel
    {
    }
}
