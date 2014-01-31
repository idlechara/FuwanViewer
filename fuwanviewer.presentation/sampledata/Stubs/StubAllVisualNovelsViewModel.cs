using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Repository;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubAllVisualNovelsViewModel : WorkspaceViewModel
    {
        public ICollection<VisualNovel> AllNovels { get; set; }

        public StubVisualNovelPreviewViewModel VisualNovelPreview { get; set; }

        public StubAllVisualNovelsViewModel()
        {
            AllNovels = new List<VisualNovel>()
            {
                new StubVisualNovel(),
                new StubVisualNovel(),
                new StubVisualNovel(),
                new StubVisualNovel()
            };

            VisualNovelPreview = new StubVisualNovelPreviewViewModel();

            base.DisplayName = "ALL NOVELS";            
        }
    }
}
