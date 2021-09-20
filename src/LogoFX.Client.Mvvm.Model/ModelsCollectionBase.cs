using System;
using LogoFX.Client.Mvvm.Model.Contracts;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// Base class for collection of models, supporting collection change notifications
    /// </summary>
    public abstract class ModelsCollectionBase : IInfoModelsCollection
    {
        /// <inheritdoc />       
        public abstract int ItemsCount { get; }

        /// <inheritdoc />        
        public abstract bool HasItems { get; }

        /// <inheritdoc />      
        public event EventHandler HasItemsChanged;

        /// <summary>
        /// Raises the items collection change event
        /// </summary>
        protected void SafeRaiseHasItemsChanged()
        {
            HasItemsChanged?.Invoke(this, new EventArgs());
        }
    }
}