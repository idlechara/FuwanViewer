using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace FuwanViewer.Presentation.ViewModels.Abstract
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// User friendly name of ViewModel
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged

    }
}
