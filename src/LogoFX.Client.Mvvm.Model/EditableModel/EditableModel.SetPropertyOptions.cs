using LogoFX.Client.Core;

namespace LogoFX.Client.Mvvm.Model
{
    /// <summary>
    /// The <see cref="EditableModel"/> set property options
    /// </summary>
    public class EditableSetPropertyOptions : SetPropertyOptions
    {
        /// <summary>
        /// True, if the model should be marked as dirty, false otherwise. The default value is <c>true</c>
        /// </summary>
        public bool MarkAsDirty { get; set; } = true;
    }
}
