using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.ViewModels.SettingGroups
{

    // THIS IS A DUMMY CLASS, WITH NOTHING WORKING YET
    // DON'T PAY IT MUCH ATTENTION
    public class ThemeSettingGroupViewModel : SettingsGroupViewModel
    {
        public List<string> Themes { get; set; }
        public string SelectedTheme 
        {
            get
            {
                return Properties.Settings.Default.SelectedTheme;
            }
            set
            {
                Properties.Settings.Default.SelectedTheme = value;
            }
        }

        #region Constructors

        public ThemeSettingGroupViewModel()
        {
            base.DisplayName = "Themes";
            CreateThemes();
        }

        #endregion // Constructors 

        void CreateThemes()
        {
            Themes = new List<string>() { "Default", "Dark" };           
        }
    }
}
