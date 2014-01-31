using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FuwanViewer.Presentation.EventBehaviours
{
    public static class ControlBehaviours
    {
        public static readonly DependencyProperty DoubleClickCommand;
        public static readonly DependencyProperty DoubleClickCommandParameter;

        // Double click command
        public static void SetDoubleClickCommand(Control o, ICommand command)
        {
            o.SetValue(DoubleClickCommand, command);
        }

        public static void GetDoubleClickCommand(Control o)
        {
            o.GetValue(DoubleClickCommand);
        }
        
        // double click command parameter
        public static void SetDoubleClickCommandParameter(Control o, object value)
        {
            o.SetValue(DoubleClickCommandParameter, value);
        }

        public static void GetDoubleClickCommandParameter(Control o)
        {
            o.GetValue(DoubleClickCommandParameter);
        }

        // STATIC CONSTRUCTOR
        static ControlBehaviours()
        {
            DoubleClickCommandParameter = EventBehaviourFactory.CreateParameterProperty("DoubleClickCommandParameter", typeof(ControlBehaviours));

            DoubleClickCommand = EventBehaviourFactory.CreateParametrizedCommandExecutionEventBehaviour(
                Control.MouseDoubleClickEvent,
                "DoubleClickCommand",
                typeof(ControlBehaviours),
                DoubleClickCommandParameter);
        }
    }
}
