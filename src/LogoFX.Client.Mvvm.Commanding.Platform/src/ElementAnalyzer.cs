using System.Windows.Input;
using System.Reflection;
using LogoFX.Client.Core;

#if NET || NETCORE || NETFRAMEWORK
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#endif

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif

namespace LogoFX.Client.Mvvm.Commanding
{
    class ElementAnalyzer
    {
        public string CommandName { get; private set; }

        public ElementAnalyzer(string commandName)
        {
            CommandName = commandName;
        }

        internal ElementAnalysisResult Analyze(DependencyObject commandTargetElement)
        {
            PropertyInfo commandProperty = null;
            var commandTargetDataContext = commandTargetElement.GetValue(FrameworkElement.DataContextProperty);
            if (commandTargetDataContext != null)
            {
                commandProperty = GetCommandProperty(commandTargetDataContext);
            }
            var canUseCommand = CanUseCommandProperty(commandProperty);
            if (!canUseCommand)
            {
                var nextElement = GetNextCommandTargetElement(commandTargetElement);    
                return new ElementAnalysisResult(nextElement);          
            }
            else
            {
                var command = (ICommand)commandProperty.GetValue(commandTargetDataContext, null);  
                return new ElementAnalysisResult(command);
            }            
        }

        private static bool CanUseCommandProperty(PropertyInfo commandProperty)
        {
            return  commandProperty != null &&
                    commandProperty.CanRead &&
                    typeof(ICommand).IsAssignableFrom(commandProperty.PropertyType);
        }
        private PropertyInfo GetCommandProperty(object commandTargetDataContext)
        {
            PropertyInfo commandProperty;
#if WINDOWS_APP || WINDOWS_PHONE_APP
            commandProperty = commandTargetDataContext.GetType().GetRuntimeProperty(CommandName);
#else
            commandProperty = commandTargetDataContext.GetType().GetProperty(CommandName);
#endif
            return commandProperty;
        }

        private DependencyObject GetNextCommandTargetElement(DependencyObject currentCommandTargetElement)
        {
            DependencyObject nextTargetElement = currentCommandTargetElement;
            DependencyObject temp;
#if NET || NETCORE || NETFRAMEWORK
            if (currentCommandTargetElement is ContextMenu)
            {
                ContextMenu cm = currentCommandTargetElement as ContextMenu;
                temp = cm.PlacementTarget;
            }
            else
#endif
            {
                temp = VisualTreeHelper.GetParent(currentCommandTargetElement);
            }
            if (temp == null)
            {
                FrameworkElement element = currentCommandTargetElement as FrameworkElement;
                if (element?.Parent == null)
                {
                    nextTargetElement = CommonProperties.GetOwner(currentCommandTargetElement) as FrameworkElement;
                }
                else
                {
                    nextTargetElement = element.Parent;
                }
            }
            else
            {
                nextTargetElement = temp;
            }
            return nextTargetElement;
        }             
    }
}