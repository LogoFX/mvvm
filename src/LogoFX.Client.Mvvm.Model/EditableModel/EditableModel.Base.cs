using System.Runtime.CompilerServices;

namespace LogoFX.Client.Mvvm.Model
{
    public partial class EditableModel<T>
    {        
        /// <summary>
        /// Compares the current and new values. If they are different,
        /// invokes the functionality which is set in the <b>options</b> parameter, 
        /// updates the respective field 
        /// and fires the property change notification.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="currentValue">The current value field reference.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="options">The set property options.</param>
        /// <param name="name">The property name.</param>
        protected void SetProperty<TProperty>(
            ref TProperty currentValue,
            TProperty newValue,
            EditableSetPropertyOptions options = null,
            [CallerMemberName] string name = "")
        {
            options = options ?? new EditableSetPropertyOptions();
            if (options.MarkAsDirty)
            {
                if (options.BeforeValueUpdate != null)
                { 
                    var prevBeforeValueUpdate = options.BeforeValueUpdate;
                    options.BeforeValueUpdate = () =>
                    {
                        prevBeforeValueUpdate();
                        MakeDirty();
                        options.BeforeValueUpdate = prevBeforeValueUpdate;
                    };
                }
                else
                {
                    options.BeforeValueUpdate = MakeDirty;
                }
            }
            base.SetProperty(ref currentValue, newValue, options, name);
        }
    }
}
