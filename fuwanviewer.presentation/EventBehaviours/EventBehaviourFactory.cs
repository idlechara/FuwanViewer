using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FuwanViewer.Presentation.EventBehaviours
{
    // I copied this code from http://blog.functionalfun.net/2008/09/hooking-up-commands-to-events-in-wpf.html
    // just so that we get 100% separation of logic (ViewModels) and Presentation (Views)
    // Under the link there's a short example of how to use this Factory.
    // Slightly extended by me, to allow creating "[eventname]command" and corresponding "[eventname]commandparameter" 
    // properties, since the one given from the website passes event's event args as a command parameter :(

    /// <summary>
    /// Used to ease up attaching commands to different events (not only the default ones)
    /// </summary>
    public static class EventBehaviourFactory
    {
        public static DependencyProperty CreateCommandExecutionEventBehaviour(RoutedEvent routedEvent, string propertyName, Type ownerType)
        {
            return DependencyProperty.RegisterAttached(
                propertyName, 
                typeof(ICommand), 
                ownerType,
                new PropertyMetadata(null, new ExecuteCommandOnRoutedEventBehaviour(routedEvent).PropertyChangedHandler));
        }

        public static DependencyProperty CreateParametrizedCommandExecutionEventBehaviour(RoutedEvent routedEvent, string propertyName, Type ownerType, DependencyProperty parameterProperty)
        {
            return DependencyProperty.RegisterAttached(
                propertyName,
                typeof(ICommand),
                ownerType,
                new PropertyMetadata(null, new ExecuteParametrizedCommandOnRoutedEventBehaviour(routedEvent, parameterProperty).PropertyChangedHandler));
        }

        public static DependencyProperty CreateParameterProperty(string propertyName, Type ownerType)
        {
            return DependencyProperty.RegisterAttached(propertyName, typeof(object), ownerType);
        }

        internal abstract class ExecuteCommandBehaviour
        {
            protected DependencyProperty _property;
            protected abstract void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue);

            protected virtual void HandleEvent(object sender, object param)
            {
                DependencyObject dp = sender as DependencyObject;
                if (dp == null)
                    return;

                ICommand command = dp.GetValue(_property) as ICommand;
                if (command == null)
                    return;

                if (command.CanExecute(param))
                {
                    command.Execute(param);
                }
            }

            /// <summary>
            /// Listens for a change in the DependencyProperty that we are assigned to, and
            /// adjusts the EventHandlers accordingly
            /// </summary>
            public void PropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            {
                // the first time the property changes,
                // make a note of which property we are supposed
                // to be watching
                if (_property == null)
                {
                    _property = e.Property;
                }

                object oldValue = e.OldValue;
                object newValue = e.NewValue;

                AdjustEventHandlers(sender, oldValue, newValue);
            }
        }

        /// <summary>
        /// An internal class to handle listening for an event and executing a command,
        /// when a Command is assigned to a particular DependencyProperty
        /// </summary>
        private class ExecuteCommandOnRoutedEventBehaviour : ExecuteCommandBehaviour
        {
            private readonly RoutedEvent _routedEvent;

            public ExecuteCommandOnRoutedEventBehaviour(RoutedEvent routedEvent)
            {
                _routedEvent = routedEvent;
            }

            /// <summary>
            /// Handles attaching or Detaching Event handlers when a Command is assigned or unassigned
            /// </summary>
            protected override void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue)
            {
                UIElement element = sender as UIElement;
                if (element == null) { return; }

                if (oldValue != null)
                {
                    element.RemoveHandler(_routedEvent, new RoutedEventHandler(EventHandler));
                }

                if (newValue != null)
                {
                    element.AddHandler(_routedEvent, new RoutedEventHandler(EventHandler));
                }
            }

            protected void EventHandler(object sender, RoutedEventArgs e)
            {
                HandleEvent(sender, e);
            }
        }

        /// <summary>
        /// Same as above, but takes parameter property to construct and pass to command as an argument
        /// </summary>
        private class ExecuteParametrizedCommandOnRoutedEventBehaviour : ExecuteCommandBehaviour
        {
            private readonly RoutedEvent _routedEvent;
            private readonly DependencyProperty _parameterProperty;

            public ExecuteParametrizedCommandOnRoutedEventBehaviour(RoutedEvent routedEvent, DependencyProperty parameterProperty)
            {
                _routedEvent = routedEvent;
                _parameterProperty = parameterProperty;
            }

            protected void EventHandler(object sender, RoutedEventArgs e)
            {
                var dp = sender as DependencyObject;
                object param = dp.GetValue(_parameterProperty);
                
                HandleEvent(sender, param);
            }

            protected override void HandleEvent(object sender, object param)
            {
                DependencyObject dp = sender as DependencyObject;
                if (dp == null)
                    return;

                ICommand command = dp.GetValue(base._property) as ICommand;
                if (command == null)
                    return;

                if (command.CanExecute(param))
                    command.Execute(param);
            }

            /// <summary>
            /// Handles attaching or Detaching Event handlers when a Command is assigned or unassigned
            /// </summary>
            protected override void AdjustEventHandlers(DependencyObject sender, object oldValue, object newValue)
            {
                UIElement element = sender as UIElement;
                if (element == null) { return; }

                if (oldValue != null)
                    element.RemoveHandler(_routedEvent, new RoutedEventHandler(EventHandler));

                if (newValue != null)
                    element.AddHandler(_routedEvent, new RoutedEventHandler(EventHandler));
            }
        }

        
    } 
}
