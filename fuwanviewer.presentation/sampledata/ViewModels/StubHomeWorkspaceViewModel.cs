using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels;

namespace FuwanViewer.Presentation.ViewModels
{
    public class StubHomeWorkspaceViewModel : HomeViewModel
    {
        public new List<VisualNovelWidgetViewModel> RecentlyViewedNovels { get; set; }

        public StubHomeWorkspaceViewModel() 
            : base(null, null, null, new HistoryList<VisualNovel>(6))
        {
            this.RecentlyViewedNovels = new List<VisualNovelWidgetViewModel>(4);
            RecentlyViewedNovels.Add(new StubVisualNovelWidgetViewModel());
            RecentlyViewedNovels.Add(new StubVisualNovelWidgetViewModel());
            RecentlyViewedNovels.Add(new StubVisualNovelWidgetViewModel());
        }
    }
}
