using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Model.VisualNovels
{
    /// <summary>
    /// Defines methods to retrieve VisualNovels and an underlying Novel cache.
    /// Also, allows to define wheter images should be cached or not.
    /// </summary>
    public interface IVisualNovelRepository : IRepository<int, VisualNovel>  
    {
        // Get() and GetAll() from IRepository.

        /// <summary>
        /// Represents wheter Images should be cached or not.
        /// </summary>
        bool CacheImages { get; set; }

        ICache<Uri, BitmapImage> ImageCache { get; }
    }
}
