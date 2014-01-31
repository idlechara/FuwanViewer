using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubHomeWorkspaceViewModel
    {
        public List<StubVisualNovelWidgetViewModel> RecentlyViewedNovels { get; set; }

        public StubHomeWorkspaceViewModel()
        {
            RecentlyViewedNovels = new List<StubVisualNovelWidgetViewModel>()
            {
                new StubVisualNovelWidgetViewModel(),
                new StubVisualNovelWidgetViewModel(),
                new StubVisualNovelWidgetViewModel(),
                new StubVisualNovelWidgetViewModel()
            };
        }
    }
}
