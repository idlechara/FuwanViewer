using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubMainWindowViewModel
    {
        public string DisplayName { get; set; }

        public ObservableCollection<WorkspaceViewModel> Workspaces { get; set; }
        public ObservableCollection<SidePanelViewModel> SidePanels { get; set; }
        public ObservableCollection<SidePanelViewModel> HiddenPanels { get; set; }
        public StubHomeWorkspaceViewModel HomeWorkspace { get; set; }
        public ControlPanelViewModel ControlPanel { get; set; }

        public StubMainWindowViewModel()
        {
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.Add(new StubAllVisualNovelsViewModel());
            Workspaces.Add(new StubVisualNovelViewModel());

            SidePanels = new ObservableCollection<SidePanelViewModel>();
            SidePanels.Add(new StubSearchPanelViewModel());

            HiddenPanels = new ObservableCollection<SidePanelViewModel>();

            ControlPanel = new StubControlPanelViewModel();

            HomeWorkspace = new StubHomeWorkspaceViewModel();

            DisplayName = "FUWANVIEWER";
        }
    }
}
