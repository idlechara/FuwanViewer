using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Services.Interfaces;

namespace FuwanViewer.Presentation.SampleData.Other
{
    public class StubVisualNovelService : IVisualNovelService
    {
        List<VisualNovel> _visualNovels;

        public bool CacheImages { get { return true; } set { } }

        public bool LimitCacheSize { get { return false; } set { } }

        public long CacheMaximumSize { get { return 102400; } set { } }

        public string CacheDirectory { get { return "C:/CacheDirectory"; } set { } }

        public string FuwaUsername { get { return "UserA"; } set { } }

        public string FuwaPassword { get { return "UserB"; } set { } }

        public StubVisualNovelService()
        {
            _visualNovels = new List<VisualNovel>()
            {
                new StubVisualNovel(),
                new StubVisualNovel(),
                new StubVisualNovel(),
                new StubVisualNovel()
            };
        }

        public IEnumerable<VisualNovel> GetAllVisualNovels()
        {
            return _visualNovels;
        }

        public IEnumerable<VisualNovel> GetFeaturedNovels()
        {
            return _visualNovels;
        }

        public VisualNovel GetVisualNovel(int id)
        {
            return _visualNovels[0];
        }
    }
}
