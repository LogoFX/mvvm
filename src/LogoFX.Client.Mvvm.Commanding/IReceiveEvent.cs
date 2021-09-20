using System;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Represents an object that is able to receive weak events.
    /// </summary>
    public interface IReceiveEvent
    {
        /// <summary>
        /// Receives the weak event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        bool ReceiveWeakEvent(EventArgs e);
    }
}
