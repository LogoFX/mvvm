using System.Collections.Generic;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Core;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Represents collection of models, supporting collection change notifications. 
    /// Supports bulk operations efficiently.
    /// </summary>
    /// <typeparam name="TItem">Type of model</typeparam>
    public class RangeModelsCollection<TItem> : ModelsCollectionBase, IModelsCollection<TItem>, IWriteRangeModelsCollection<TItem>
    {
        private RangeObservableCollection<TItem> Items { get; } = new RangeObservableCollection<TItem>();

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
            Items.AddRange(items);
            SafeRaiseHasItemsChanged();
        }

        /// <inheritdoc />        
        public void Clear()
        {
            Items.Clear();            
            SafeRaiseHasItemsChanged();
        }

        /// <inheritdoc />        
        public void AddRange(IEnumerable<TItem> items)
        {
            Items.AddRange(items);
            SafeRaiseHasItemsChanged();
        }

        /// <inheritdoc />       
        public void RemoveRange(IEnumerable<TItem> items)
        {
            Items.RemoveRange(items);
            SafeRaiseHasItemsChanged();
        }
    }
}