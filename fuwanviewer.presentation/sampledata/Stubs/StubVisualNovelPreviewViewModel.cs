using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubVisualNovelPreviewViewModel
    {
        public string DisplayName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public BitmapImage Cover {get; set;}
        public BitmapImage MainArt { get; set; }

        public StubVisualNovelPreviewViewModel()
        {
            var vn = new StubVisualNovel();
            DisplayName = Title = vn.Title;
            Description = vn.Description;
            Cover = vn.Cover;
            MainArt = vn.MainArt;
            Tags = vn.Tags;
        }
    }
}
