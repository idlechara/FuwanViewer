using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubVisualNovelWidgetViewModel : WorkspaceViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Company { get; set; }
        public string TeamListing { get; set; }
        public string Notes { get; set; }
        public string TroubleshootText { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Tag> Tags { get; set; }
        public BitmapImage MainArt { get; set; }
        public ICollection<BitmapImage> Screenshoots { get; set; }

        public string FuwanovelUrl { get; set; }
        public string TranslationHomepageUrl { get; set; }
        public string FaqUrl { get; set; }
        public string ForumUrl { get; set; }
        public string WalkthroughUrl { get; set; }


        public int EgsScore { get; set; }
        public int EgsVotes { get; set; }


        public StubVisualNovelWidgetViewModel()
        {
            var vn = new StubVisualNovel();

            DisplayName = Title = vn.Title;
            Description = vn.Description;
            Summary = vn.Summary;
            Company = vn.Company;
            MainArt = vn.MainArt;
            Screenshoots = vn.Screenshoots;
            TeamListing = vn.TeamListing;
            Notes = vn.Notes;
            ReleaseDate = vn.ReleaseDate;
            TroubleshootText = vn.TroubleshootText;
            Tags = vn.Tags;


            EgsScore = vn.EgsScore;
            EgsVotes = vn.EgsVotes;

            FuwanovelUrl = vn.FuwanovelUrl;
            TranslationHomepageUrl = vn.TranslationHomepageUrl;
            WalkthroughUrl = vn.WalkthroughUrl;
            ForumUrl = vn.ForumUrl;
            FaqUrl = vn.FaqUrl;
        }
    }
}
