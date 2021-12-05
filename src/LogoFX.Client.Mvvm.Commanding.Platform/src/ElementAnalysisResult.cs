using System.Windows.Input;

#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
#endif

#if NETFX_CORE
using Windows.UI.Xaml;
#endif

namespace LogoFX.Client.Mvvm.Commanding
{
    class ElementAnalysisResult
    {
        public ElementAnalysisResult(           
            ICommand command)
        {
            CanUseCommand = true;
            Command = command;
            NextElement = null;
        }

        public ElementAnalysisResult(           
            DependencyObject nextElement)
        {
            CanUseCommand = false;
            Command = null;
            NextElement = nextElement;
        }

        public bool CanUseCommand { get; private set; }
        public ICommand Command { get; private set; }
        public DependencyObject NextElement { get; private set; }
    }
}