using System.Collections.Generic;
using System.Collections.ObjectModel;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents collection of models, supporting collection change notifications
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public class ModelsCollection<TItem> : ModelsCollectionBase, IModelsCollection<TItem>
    {
        private ObservableCollection<TItem> Items { get; } = new ObservableCollection<TItem>();

        IEnumerable<TItem> IReadModelsCollection<TItem>.Items => Items;

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <value>
        /// The items count.
        /// </value>
        public override int ItemsCount => Items.Count;

        /// <summary>
        /// Gets a value indicating whether this instance has items.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has items; otherwise, <c>false</c>.
        /// </value>
        public override bool HasItems => ItemsCount > 0;

        /// <inheritdoc />       
        public void Add(TItem item)
        {
            Items.Add(item);
            SafeRaiseHasItemsChanged();
        }

        /// <inheritdoc />       
        public void Remove(TItem item)
        {
            Items.Remove(item);
            SafeRaiseHasItemsChanged();
        }

        /// <inheritdoc />        
        public void Update(IEnumerable<TItem> items)
        {
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
            SafeRaiseHasItemsChanged();
        }

        /// <inheritdoc />       
        public void Clear()
        {
            Items.Clear();
            SafeRaiseHasItemsChanged();
        }
    }
}
