using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Presentation.ViewModels.SettingGroups;

namespace FuwanViewer.Presentation.ViewModels
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        public ICommand CancelCommand { get; set; }
        public ICommand ApplyCommand { get; set; }

        public ICollection<SettingsGroupViewModel> SettingGroups { get; set; }

        public SettingsDialogViewModel()
        {
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = null;

            SettingGroups = 
                new List<SettingsGroupViewModel>() { 
                    new GeneralSettingGroupViewModel(),
                    new CacheSettingsGroupViewModel()
                    };
        }

        #region Command Actions

        void Apply(object param)
        {
            foreach (var group in SettingGroups)
            {
                group.SaveSettings();
            }
        }

        #endregion // Command Actions
    }
}
