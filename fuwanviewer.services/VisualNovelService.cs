using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using FuwanViewer.Model.Infrastructure;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository;
using FuwanViewer.Repository.Fake;
using FuwanViewer.Repository.Fake.Proxy;
using FuwanViewer.Repository.Proxy;
using FuwanViewer.Services.Interfaces;
using System.Windows.Media.Imaging;

namespace FuwanViewer.Services
{
    /// <summary>
    /// Provides basic methods and properties for retrieving (later maybe more)
    /// of VisualNovels.
    /// </summary>
    [DataContract]
    [KnownType(typeof(FakeFuwaVNRepository))]
    [KnownType(typeof(FuwaVNRepository))]
    [KnownType(typeof(VisualNovelProxy))]
    [KnownType(typeof(FakeVisualNovelProxy))]
    public class VisualNovelService : IVisualNovelService
    {
        #region Fields and Properties

        [DataMember]
        IVisualNovelRepository _repository;

        public string FuwaUsername 
        {
            get
            {
                var repository = _repository as IRequiresCredentials;
                if (repository != null)
                    return repository.Username;
                else
                    return String.Empty;
            }
            set
            {
                var repository = _repository as IRequiresCredentials;
                if (repository != null)
                    repository.Username = value;
            }
        }
        public string FuwaPassword
        {
            get
            {
                var repository = _repository as IRequiresCredentials;
                if (repository != null)
                    return repository.Password;
                else
                    return String.Empty;
            }
            set
            {
                var repository = _repository as IRequiresCredentials;
                if (repository != null)
                    repository.Password = value;
            }
        }

        public bool CacheImages 
        {
            get { return _repository.CacheImages; }
            set { _repository.CacheImages = value; }
        }
        public bool LimitCacheSize
        {
            get 
            { 
                var cache = _repository.ImageCache as IFileCache<Uri, BitmapImage>;
                if (cache != null)
                    return cache.LimitSize;
                else
                    return false;
            }
            set
            {
                var cache = _repository.ImageCache as IFileCache<Uri, BitmapImage>;
                if (cache != null)
                    cache.LimitSize = value;
            }
        }
        public long CacheMaximumSize
        {
            get 
            { 
                var cache = _repository.ImageCache as IFileCache<Uri, BitmapImage>;
                if (cache != null)
                    return cache.MaximumSize;
                else
                    return 0;
            }
            set
            {
                var cache = _repository.ImageCache as IFileCache<Uri, BitmapImage>;
                if (cache != null)
                    cache.MaximumSize = value;
            }
        }
        public string CacheDirectory
        {
           get 
            { 
                var cache = _repository.ImageCache as IFileCache<Uri, BitmapImage>;
                if (cache != null)
                    return cache.Directory;
                else
                    return null;
            }
            set
            {
                var cache = _repository.ImageCache as IFileCache<Uri, BitmapImage>;
                if (cache != null)
                    cache.Directory = value;
            }
        }

        #endregion // Fields and Properties

        #region Constructors

        public VisualNovelService(IVisualNovelRepository repository)
        {
            this._repository = repository;
        }

        #endregion // Constructors

        #region Public Interface

        /// <summary>
        /// Returns a visual novel with a given title, or null
        /// if no such Visual Novel exists
        /// </summary>
        public VisualNovel GetVisualNovel(int id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<VisualNovel> GetAllVisualNovels()
        {
            return _repository.GetAll();
        }

        public IEnumerable<VisualNovel> GetFeaturedNovels()
        {
            var allNovels = _repository.GetAll();
            return allNovels.Where((vn) => vn.IsFeatured).ToList();
        }

        #endregion // Public Interface
    }
}
