#if WINDOWS_UWP || NETFX_CORE
using Windows.UI.Interactivity;
using Windows.UI.Xaml;
using TriggerBase = Windows.UI.Interactivity.TriggerBase;
using TriggerCollection = Windows.UI.Interactivity.TriggerCollection;
#endif

namespace System.Windows.Interactivity
{
    /// <summary>
    /// <see cref="FrameworkTemplate"/> for InteractivityElements instance
    /// <remarks>can't use <see cref="FrameworkTemplate"/> directly due to some internal abstract member</remarks>
    /// </summary>
    public class InteractivityTemplate : DataTemplate
    {}
}
