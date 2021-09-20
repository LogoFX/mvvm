using System;
using System.Windows.Input;
using System.Reflection;

#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
#endif

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
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