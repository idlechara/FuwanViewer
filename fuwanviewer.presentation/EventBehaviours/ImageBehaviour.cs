using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace FuwanViewer.Presentation.EventBehaviours
{
    public static class ImageBehaviour
    {
        #region SizeChanged
        public static readonly DependencyProperty SizeChangedCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(Image.SizeChangedEvent, "SizeChangedCommand", typeof(ImageBehaviour));

        public static void SetSizeChangedCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(SizeChangedCommand, value);
        }

        public static ICommand GetTextChangedCommand(DependencyObject o)
        {
            return o.GetValue(SizeChangedCommand) as ICommand;
        }
        #endregion //Sizechanged

    }

}
