namespace LogoFX.Client.Mvvm.ViewModel
{
    /// <summary>
    /// Represents setup options for <see cref="WrappingCollection"/>
    /// </summary>
    public class SetupOptions
    {
        /// <summary>
        /// Gets or sets the values indicating whether to use bulk mode.
        /// </summary>
        public bool IsBulk { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to use concurrent data structures.
        /// </summary>
        public bool IsConcurrent { get; set; }

        /// <summary>
        /// Configures the setup to use bulk mode.
        /// </summary>
        /// <returns></returns>
        public SetupOptions UseBulk()
        {
            IsBulk = true;
            return this;
        }

        /// <summary>
        /// Configures the setup to use concurrent data structures.
        /// </summary>
        /// <returns></returns>
        public SetupOptions UseConcurrent()
        {
            IsConcurrent = true;
            return this;
        }
    }
}
