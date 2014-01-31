using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    class StubFeaturedVisualNovelsViewModel : WorkspaceViewModel
    {
        public ICollection<StubVisualNovelWidgetViewModel> FeaturedNovels { get; set; }

        public StubFeaturedVisualNovelsViewModel()
        {
            FeaturedNovels = new List<StubVisualNovelWidgetViewModel>()
            {
                new StubVisualNovelWidgetViewModel(),
                new StubVisualNovelWidgetViewModel(),
            };

            base.DisplayName = "Featured Novels";
        }
    }
}
