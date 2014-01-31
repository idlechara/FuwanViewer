using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubControlPanelViewModel : ControlPanelViewModel 
    {
        public StubControlPanelViewModel() : base(null, null, null, null)
        {
            this.DisplayName = "Control Panel";
        }
    }
}
