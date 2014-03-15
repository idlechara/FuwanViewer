using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Presentation.SampleData.Other
{
    public class StubVisualNovel : VisualNovel
    {
        public StubVisualNovel()
        {
            Title = "ef - a fairy tale of the two.";
            Summary = @"Amamiya Yuuko, a mysterious girl dressed like a nun, and Himura Yuu, a mysterious gentleman who is somehow attached to the church where Yuuko first appears, are having a reunion in a church during Christmas time. Despite her attire, Yuuko is not affiliated with the church. She always appears generally out of no where, and disappears just as quickly in various places throughout the story to talk with Hiro or other characters and give them advice. Yuuko and Yuu reminisce about the past and remember events of the previous year around the same time at the beginning of the first chapter of the story. Yuuko hints of events that are revealed throughout Ef: A Fairy Tale of the Two..";
            Description = @"This is a short description of this fantastic visual novel";
            Company = "Minori";
            ReleaseDate = new DateTime(2013, 2, 13);
            Tags = new List<Tag>();
            Tags.Add(Tag.Action);
            Tags.Add(Tag.AllAges);
            Tags.Add(Tag.DarkThemes);
            EgsScore = 10;
            EgsVotes = 100;
            TeamListing = "The fuwanovel team.";
            Notes = "Well, this is a dummy data, so, it doesn't matter what text is here.";
            TroubleshootText = "If you have any problems, contact to the admin?";
            Cover = new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef0.jpg"));
            MainArt = new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/EfMainArt.jpg"));
            Tags = new List<Tag>()
            {
                Tag.Drama,
                Tag.Comedy,
                Tag.Horror,
                Tag.Mystery
            };

            Screenshoots = new List<BitmapImage>(8)
            {
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef1.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef2.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef3.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef4.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef5.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef6.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef7.jpg")),
                new BitmapImage(new Uri("pack://application:,,,/FuwanViewer.Presentation;component/SampleData/Images/Ef8.jpg"))
            };

            FuwanovelUrl = "http://fuwanovel.org/novels/rewrite";
            TranslationHomepageUrl = "http://amaterasu.tindabox.net/";
            WalkthroughUrl = "http://forums.fuwanovel.org/index.php?/topic/689-rewrite/";
            ForumUrl = "http://forums.fuwanovel.org";
            FaqUrl = "http://gamefaqs.com";

            Torrent = new Torrent();

            Torrent.Name = "Torrent name";
            Torrent.RequiresInstall = true;
            Torrent.RequiresJpLocale = false;
            Torrent.Size= 1020;
            Torrent.Url = "http://forums.fuwanovel.org/index.php?/topic/689-rewrite/";
        }
    }
}
