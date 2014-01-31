using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FuwanViewer.Presentation.ViewModels.Abstract
{
    /// <summary>
    /// View model exposing CloseCommand
    /// </summary>
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Fields and Properties

        RelayCommand _closeCommand;

        /// <summary>
        /// Returns the command that, when invoked, raises RequestClose 
        /// event in order to remove workspace from interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());

                return _closeCommand;
            }
        }

        #endregion

        #region Event - Request Close

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        protected void OnRequestClose()
        {
            if (RequestClose != null)
                RequestClose(this, EventArgs.Empty);
        }

        #endregion // Event - Request Close

        protected WorkspaceViewModel() { }

        /// <summary>
        /// Refreshes (Updates) workspace.
        /// </summary>
        virtual public void Refresh() { }
    }
}
