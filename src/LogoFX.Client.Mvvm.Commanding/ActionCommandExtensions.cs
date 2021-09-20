using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Windows.Input;
using LogoFX.Client.Core;

namespace LogoFX.Client.Mvvm.Commanding
{
    /// <summary>
    /// Extension methods for <see cref="ActionCommand"/>
    /// </summary>
    public static class ActionCommandExtensions
    {
        /// <summary>
        /// Queries for command state according to the property notifications
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="notifiable">Source of property notifications</param>
        /// <returns>Command after setup</returns>
        public static T RequeryOnPropertyChanged<T>(this T command, INotifyPropertyChanged notifiable)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotDefault(notifiable, "notifiable");

            command.AddDisposable(Observable
                .FromEventPattern<PropertyChangedEventHandler,PropertyChangedEventArgs>(a=>notifiable.PropertyChanged+=a, a=>notifiable.PropertyChanged-=a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs)));
            return command;
        }        

        /// <summary>
        /// Queries for command state according to the specified property notifications by expression
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="notifiable">Source of property notifications</param>
        /// <param name="propertySelector">Property selector expression</param>
        /// <returns>Command after the setup</returns>
        public static T RequeryOnPropertyChanged<T>(this T command,
            INotifyPropertyChanged notifiable, Expression<Func<object>> propertySelector)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(notifiable, "notifiable");
            Guard.ArgumentNotNull(propertySelector, "propertySelector");

            command.AddDisposable(Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(a => notifiable.PropertyChanged += a, a => notifiable.PropertyChanged -= a)
                .Where(a=>a.EventArgs.PropertyName == propertySelector.GetPropertyName())
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs)));

            return command;
        }

        /// <summary>
        /// Queries for command state according to another command state
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="relatedCommand">Related command</param>
        /// <returns>Command after the setup</returns>
        public static T RequeryOnCommandCanExecuteChanged<T>(this T command, ICommand relatedCommand)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(relatedCommand, "relatedCommand");

            command.AddDisposable(Observable
                .FromEventPattern<EventHandler, EventArgs>(a => relatedCommand.CanExecuteChanged += a, a => relatedCommand.CanExecuteChanged -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs)));

            return command;
        }

        /// <summary>
        /// Queries for command state according to another command execution
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="relatedCommand">Related command</param>
        /// <returns>Command after the setup</returns>
        public static T RequeryOnCommandExecuted<T>(this T command, IActionCommand relatedCommand)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(relatedCommand, "relatedCommand");

            command.AddDisposable(Observable
                .FromEventPattern<EventHandler<CommandEventArgs>,CommandEventArgs>(a => relatedCommand.CommandExecuted += a, a => relatedCommand.CommandExecuted -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs)));

            return command;
        }

        /// <summary>
        /// Queries for command state according to its execution
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <returns>Command after the setup</returns>
        public static T RequeryWhenExecuted<T>(this T command)
            where 
                T : IActionCommand
        {
            Guard.ArgumentNotDefault(command, "command");

            command.AddDisposable(Observable
                .FromEventPattern<EventHandler<CommandEventArgs>, CommandEventArgs>(a => command.CommandExecuted += a, a => command.CommandExecuted -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs)));

            return command;
        }

        /// <summary>
        /// Queries for command state according to the collection notifications
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="collection">Source of collection notifications</param>
        /// <returns>Command after setup</returns>
        public static T RequeryOnCollectionChanged<T>(this T command, INotifyCollectionChanged collection)
            where 
                T : ICommand, IReceiveEvent
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(collection, "collection");

            command.AddDisposable(Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(a => collection.CollectionChanged += a, a => collection.CollectionChanged -= a)
                .Subscribe(a => command.ReceiveWeakEvent(a.EventArgs)));

            return command;
        }

        /// <summary>
        /// Sets image of the command
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="image">Image Uri</param>
        /// <returns>Command after the setup</returns>
        public static T WithImage<T>(this T command, Uri image)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(image, "image");
            command.ImageUri = image; 

            return command;
        }

        /// <summary>
        /// Sets name of the command
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="name">Name</param>
        /// <returns>Command after the setup</returns>
        public static T WithName<T>(this T command, string name)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(name, "name");
            command.Name = name;

            return command;
        }

        /// <summary>
        /// Sets description of the command
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="command">Command</param>
        /// <param name="description">Description</param>
        /// <returns>Command after the setup</returns>
        public static T WithDescription<T>(this T command, string description)
            where
                T : IExtendedCommand
        {
            Guard.ArgumentNotDefault(command, "command");
            Guard.ArgumentNotNull(description, "description");
            command.Description = description;

            return command;
        }

        private static void AddDisposable<T>(this T command, IDisposable disposable) where
            T : ICommand, IReceiveEvent
        {
            if (command is IDisposableCollection disposableCollection)
            {
                disposableCollection.Add(disposable);
            }
        }
    }
}
