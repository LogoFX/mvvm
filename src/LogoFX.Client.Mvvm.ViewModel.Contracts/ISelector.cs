namespace LogoFX.Client.Mvvm.ViewModel.Contracts
{
    /// <summary>
    /// Selector that supports selection of items.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    public interface ISelector<out T> : ISelector, IHaveSelectedItem<T>, IHaveSelectedItems<T> where T : ISelectable
    {
    }

    /// <summary>
    /// Provides various options for selection.
    /// </summary>
    public interface ISelector : IHaveSelectedItem, IHaveSelectedItems, IHaveSelectionPredicate, INotifySelectionChanged, ISelect, IUnselect
    {
    }
}