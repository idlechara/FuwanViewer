using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Presentation.ViewModels.SearchGroups;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubSearchPanelViewModel : SidePanelViewModel
    {
        public ICollection<SearchGroupViewModel> SearchGroups { get; set; }

        public StubSearchPanelViewModel()
        {
            DisplayName = "Search Panel";

            SearchGroups = new List<SearchGroupViewModel>();
            SearchGroups.Add(new TitleSearchGroupViewModel());
            SearchGroups.Add(new CompanySearchGroupViewModel());
            SearchGroups.Add(new TagSearchGroupViewModel());
            SearchGroups.Add(new OtherSearchGroupViewModel());
        }
    }
}
