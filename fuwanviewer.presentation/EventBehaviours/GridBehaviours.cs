using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FuwanViewer.Presentation.EventBehaviours
{
    public static class GridBehaviours
    {
        public static readonly DependencyProperty LeftMouseButtonUpCommand;

        // left mouse button up
        public static void SetLeftMouseButtonUpCommand(Grid o, object value)
        {
            o.SetValue(LeftMouseButtonUpCommand, value);
        }

        public static void GetLeftMouseButtonUpCommand(Grid o)
        {
            o.GetValue(LeftMouseButtonUpCommand);
        }

        static GridBehaviours()
        {
            LeftMouseButtonUpCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(Grid.MouseLeftButtonUpEvent, "LeftMouseButtonUpCommand", typeof(GridBehaviours));
        }
    }
}
