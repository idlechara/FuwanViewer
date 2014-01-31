using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuwanViewer.Presentation.ViewModels.Abstract
{
    /// <summary>
    /// View model exposing toggling interface
    /// </summary>
    public abstract class SidePanelViewModel : ViewModelBase
    {
        ICommand _toggleCommand;

        public ICommand ToggleCommand
        {
            get
            {
                if (_toggleCommand == null)
                    _toggleCommand = new RelayCommand((param) => OnRequestToggle());

                return _toggleCommand;
            }
        }

        public event EventHandler RequestToggle;

        void OnRequestToggle()
        {
            if (RequestToggle != null)
                RequestToggle(this, EventArgs.Empty);
        }
    }
}
