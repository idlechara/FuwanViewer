using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FuwanViewer.Presentation.EventBehaviours
{
    /// <summary>
    /// Static class extending Window class by adding ClosingCommand
    /// </summary>
    /// <remarks>
    /// This could NOT be done with event behaviour factory, because 'Closing' event is 
    /// not a routed event :(
    /// </remarks>
    public static class WindowBehaviours
    {
        public static readonly DependencyProperty ClosingCommandProperty;

        public static void SetClosingCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(ClosingCommandProperty, value);
        }

        public static ICommand GetClosingCommand(DependencyObject o)
        {
            return (ICommand)o.GetValue(ClosingCommandProperty);
        }

        static WindowBehaviours()
        {
            ClosingCommandProperty = DependencyProperty.RegisterAttached("ClosingCommand", typeof(ICommand), typeof(WindowBehaviours), new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));
        }

        private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;

            if (window != null)
            {
                if (e.NewValue != null)
                {
                    window.Closing += Window_Closing;
                }
                else
                {
                    window.Closing -= Window_Closing;
                }
            }
        }

        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            ICommand closing = GetClosingCommand(sender as Window);
            if (closing != null)
            {
                closing.Execute(null);
            }
        }
    }
}
