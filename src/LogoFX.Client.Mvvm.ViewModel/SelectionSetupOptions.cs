using System;

namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents setup options for <see cref="WrappingCollection.WithSelection"/>
    /// </summary>
    public class SelectionSetupOptions : SetupOptions
    {
        private const SelectionMode DefaultSelectionMode = SelectionMode.ZeroOrMore;

        /// <summary>
        /// Gets the selection mode.
        /// </summary>
        public SelectionMode SelectionMode { get; private set; } = DefaultSelectionMode;

        /// <summary>
        /// Gets the selection predicate.
        /// </summary>
        public Predicate<object> SelectionPredicate { get; private set; }       

        /// <summary>
        /// Configures the setup to use the provided selection mode.
        /// </summary>
        /// <param name="selectionMode">The selection mode.</param>
        /// <returns></returns>
        public SelectionSetupOptions UseSelectionMode(SelectionMode selectionMode)
        {
            SelectionMode = selectionMode;
            return this;
        }

        /// <summary>
        /// Configures the setup to use the provided selection predicate.
        /// </summary>
        /// <param name="selectionPredicate">The selection predicate.</param>
        /// <returns></returns>
        public SelectionSetupOptions UseSelectionPredicate(Predicate<object> selectionPredicate)
        {
            SelectionPredicate = selectionPredicate;
            return this;
        }

        /// <summary>
        /// Configures the setup to use bulk mode.
        /// </summary>
        /// <returns></returns>
        public new SelectionSetupOptions UseBulk()
        {
            base.UseBulk();
            return this;
        }

        /// <summary>
        /// Configures the setup to use concurrent data structures.
        /// </summary>
        /// <returns></returns>
        public new SelectionSetupOptions UseConcurrent()
        {
            base.UseConcurrent();
            return this;
        }
    }
}