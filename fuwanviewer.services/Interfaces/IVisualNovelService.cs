using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Services.Interfaces
{
    public interface IVisualNovelService
    {
        IEnumerable<VisualNovel> GetAllVisualNovels();
        IEnumerable<VisualNovel> GetFeaturedNovels();
        VisualNovel GetVisualNovel(int id);

        bool CacheImages { get; set; }
        bool LimitCacheSize { get; set; }
        long CacheMaximumSize { get; set; }
        string CacheDirectory { get; set; }

        string FuwaUsername { get; set; }
        string FuwaPassword { get; set; }
    }
}
