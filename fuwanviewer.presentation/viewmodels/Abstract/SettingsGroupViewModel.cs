using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Presentation.ViewModels.Abstract
{
    /// <summary>
    /// Base class for all SettingsGroups' View Models.
    /// </summary>
    public class SettingsGroupViewModel : ViewModelBase
    {
        /// <summary>
        /// Persists all settings associated with this SettingsGroup.
        /// </summary>
        virtual public void SaveSettings() { }
    }
}
