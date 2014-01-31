using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Fuwanovel;
using FuwanViewer.Model.Infrastructure;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository.Cache;
using FuwanViewer.Repository.Proxy;

namespace FuwanViewer.Repository
{
    /// <summary>
    /// VisualNovelRepository that uses Fuwanovel.org as a source of Visual Novels.
    /// </summary>
    /// <remarks>
    /// Since Fuwanovel API requires all requests to be authenticated, FuwaVNRepository 
    /// implements IRequiresCredentials interface.
    /// 
    /// FuwaVNRepository internally caches objects returned by Fuwanovel's API and set them to be fresh
    /// for some (minimum) 'CacheDuration'. Cache is assumed to return objects if and only if they are still fresh,
    /// therefore. If cache doesn't return any object, then a HttpRequest is sent to Fuwanovel in order to
    /// retrieve the novels and cache them again.
    /// 
    /// As a final result, both Get() and GetAll() return objects, which are up-to-date with some "CacheDuration" tolerance.
    /// 
    /// NOTE: It is left for cache object to decide wheter the object is fresh or not, however they must hold it fresh
    /// for at least "CacheDuration".
    ///  </remarks>
    [DataContract(IsReference=true)]
    [KnownType(typeof(FuwanovelCache))]
    [KnownType(typeof(SimpleImageCache))]
    [KnownType(typeof(NullImageCache))]
    public class FuwaVNRepository : IVisualNovelRepository, IRequiresCredentials
    {
        #region Fields and Properties

        [DataMember]
        internal FuwanovelCache _fuwanovelCache;
        [DataMember]
        internal ICache<Uri, BitmapImage> _imageCache;
        [DataMember]
        private string _username;
        [DataMember]
        private string _password;
        
        // IRequiresCredentials
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                FuwanovelAPI.Username = value;
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                FuwanovelAPI.Password = value;
            }
        }

        // IVisualNovelRepository
        public bool CacheImages 
        { 
            get
            {
                return !(_imageCache is NullImageCache);
            }
            set
            {
                if (value != CacheImages)
                {
                    if (value == true)
                        _imageCache = new SimpleImageCache();
                    else
                    {
                        if (_imageCache is IDisposable)
                            (_imageCache as IDisposable).Dispose();
                        _imageCache = new NullImageCache();
                    }
                }
            }
        }
        public ICache<Uri, BitmapImage> ImageCache { get { return _imageCache; } }
        
        #endregion // Fields and Properties

        #region Constructors

        public FuwaVNRepository() : this (new SimpleImageCache()) {}

        public FuwaVNRepository(ICache<Uri, BitmapImage> imageCache)
        {
            _fuwanovelCache = new FuwanovelCache(this);
            _imageCache = imageCache;
        }

        #endregion // Constructors

        #region Public Interface

        public VisualNovel Get(int id)
        {
            // if cached -> return cache
            var cached = _fuwanovelCache.Retrieve(id);
            if (cached != null)
                return VisualNovelProxyFactory.CreateProxy(this, cached);
            // else call API and add entry to cache
            else
            {
                var fresh = FuwanovelAPI.GetNovel(id);
                if (fresh == null)
                    return null;
                _fuwanovelCache.Store(fresh.id, fresh);
                return VisualNovelProxyFactory.CreateProxy(this, fresh);
            }
        }

        public IEnumerable<VisualNovel> GetAll()
        {
            List<VisualNovelProxy> result = new List<VisualNovelProxy>();

            // if cached -> return cache
            var cached = _fuwanovelCache.RetrieveAll();
            if (cached != null)
            {
                foreach (var item in cached)
                {
                    result.Add(VisualNovelProxyFactory.CreateProxy(this, item));
                }
                return result;
            }
            // else call API and synchronize cache
            else
            {
                var fuwaNovels = FuwanovelAPI.GetAllNovels();
                if (fuwaNovels == null)
                    return new List<VisualNovel>();
                _fuwanovelCache.Synchronize(fuwaNovels);
                return fuwaNovels.ConvertAll(
                    (fuwaVn) => VisualNovelProxyFactory.CreateProxy(this, fuwaVn));
            }
        }

        public bool Authenticate()
        {
            return false;
        }

        #endregion // Public Interface

        #region Data Contract Serialization

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            FuwanovelAPI.Username = _username;
            FuwanovelAPI.Password = _password;
        }

        #endregion Data Contract Serialization
    }

    class FuwaNovelEqualityComparer : IEqualityComparer<FuwaVisualNovel>
    {
        public bool Equals(FuwaVisualNovel x, FuwaVisualNovel y)
        {
            return x.id == y.id;
        }

        public int GetHashCode(FuwaVisualNovel obj)
        {
            return obj.name.GetHashCode();
        }
    }
}
