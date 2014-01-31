using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository.Cache;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Repository.Proxy
{
    /// <summary>
    /// Serializable proxy class implementing Lazy Load pattern on all the images.
    /// (download will be delayed untill bitmaps are needed by application)
    /// </summary>
    /// <remarks>
    /// Visual novel proxy internally references and uses ImageCache of parent
    /// IVisualNovelRepository.
    /// 
    /// All images have value null untill they are loaded, at which point PropertyChanged
    /// event is raised. By loading an image, I mean loading it into memory, which can be 
    /// either from a cache (e.g. based on file system) or directly from the Internet.
    /// </remarks>
    [DataContract]
    public class VisualNovelProxy : VisualNovel
    {     
        #region Fields and Properties

        Dictionary<ResourceType, bool> _haveLoaded = new Dictionary<ResourceType, bool>();
        Dictionary<ResourceType, BitmapImage> _resourceImages = new Dictionary<ResourceType, BitmapImage>();

        [DataMember]
        FuwaVNRepository _parentRepository;

        [DataMember]
        Dictionary<ResourceType, Uri> _resourceUris = new Dictionary<ResourceType, Uri>();        

        public override BitmapImage Cover
        {
            get
            {
                var resource = ResourceType.Cover;
                if (_haveLoaded[resource] == false)
                {
                    LoadResource(resource);
                }
                return _resourceImages[resource];
            }
        }

        public override BitmapImage MainArt
        {
            get
            {
                var resource = ResourceType.MainArt;
                if (_haveLoaded[resource] == false)
                {
                    LoadResource(resource);
                }
                return _resourceImages[resource];
            }
        }

        public override ICollection<BitmapImage> Screenshoots
        {
            get
            {
                List<ResourceType> screenshootResources = ResourceTypeFactory.GetScreenshootResources();

                foreach (ResourceType screenshoot in screenshootResources)
                {
                    if (_haveLoaded.ContainsKey(screenshoot) && _haveLoaded[screenshoot] == false)
                    {
                        LoadResource(screenshoot);
                    }
                }

                List<BitmapImage> result = new List<BitmapImage>();
                foreach (ResourceType screenshoot in screenshootResources)
                {
                    result.Add(_resourceImages[screenshoot]);
                }
                return result;
            }
        }

        private ICache<Uri, BitmapImage> ImageCache 
        { 
            get { return _parentRepository._imageCache; } 
        }

        #endregion // Fields and Properties

        #region Constructors

        /// <summary>
        /// Creates a VisualNovelProxy with default data and specified 
        /// coverUri and screenshootUris
        /// </summary>
        public VisualNovelProxy(FuwaVNRepository parentRepository, Uri coverUri, Uri mainArtUri, List<Uri> screenshootUris)
        {
            this._parentRepository = parentRepository;

            // Initialize Resources
            InitializeResource(ResourceType.Cover, coverUri);
            InitializeResource(ResourceType.MainArt, mainArtUri);

            var screenshootResources = ResourceTypeFactory.GetScreenshootResources();
            for (int i = 0; i < screenshootResources.Count; i++)
            {
                if (screenshootUris.Count - 1 < i)
                    break;
                else
                    InitializeResource(screenshootResources[i], screenshootUris[i]);
            }
        }

        #endregion //Constructors

        #region Data Contract Serialization

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            _resourceImages = new Dictionary<ResourceType, BitmapImage>();
            _haveLoaded = new Dictionary<ResourceType, bool>();

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                if (_resourceUris.ContainsKey(resource))
                    InitializeResource(resource, _resourceUris[resource]);
            }
        }

        #endregion // Data Contract Serialization

        #region Helper functions

        /// <summary>
        /// Initializes resource as 'not loaded' and associates it with given Uri.
        /// </summary>
        private void InitializeResource(ResourceType resource, Uri uri)
        {
            // set corresponding uri field
            _resourceUris[resource] = uri;
            _resourceImages[resource] = null;
            _haveLoaded[resource] = false;
        }

        /// <summary>
        /// Loads specified resource into memory, either from ImageCache or the Internet.
        /// </summary>
        /// <remarks>
        /// If download fails, image is kept null and Resource is set as not loaded,
        /// hence any future requests to the resource will attempt to download it again.
        /// </remarks>
        private async void LoadResource(ResourceType resource)
        {
            _haveLoaded[resource] = true;

            // if url == null => return null
            if (_resourceUris[resource] == null) 
                return;
            
            // if url is in cache => use cached image
            BitmapImage image = ImageCache.Retrieve(_resourceUris[resource]);
            if (image != null)
            {
                _resourceImages[resource] = image;
            }
            else
            {
                image = await Task<BitmapImage>.Run(() => CreateBitmapFromWebUrl(_resourceUris[resource]));
                
                // if download failed
                if (image == null)
                {
                    _haveLoaded[resource] = false;
                }
                else
                {
                    ImageCache.Store(_resourceUris[resource], image);
                    _resourceImages[resource] = image;
                }
            }

            // update Application
            base.OnPropertyChanged(GetPropertyName(resource));
        }

        /// <summary>
        /// Returns name of property associated with given resource type.
        /// </summary>
        private string GetPropertyName(ResourceType resource)
        {
            switch (resource)
            {
                case ResourceType.Cover:
                    return "Cover";
                case ResourceType.MainArt:
                    return "MainArt";
                case ResourceType.Screenshoot1:
                case ResourceType.Screenshoot2:
                case ResourceType.Screenshoot3:
                case ResourceType.Screenshoot4:
                case ResourceType.Screenshoot5:
                case ResourceType.Screenshoot6:
                case ResourceType.Screenshoot7:
                case ResourceType.Screenshoot8:
                    return "Screenshoots";
                default:
                    throw new ArgumentException("Unknown resource value");
            }
        }

        /// <summary>
        /// Creates string that can be used as a file or directory name
        /// based on parameter string.
        /// </summary>
        private string CreateSafeString(string param)
        {
            List<char> forbiddenChars = new List<char>() { '/', '\'', ':', '*', '?', '<', '>', '|' };
            StringBuilder sb = new StringBuilder(param.Length);
            for (int i = 0; i < param.Length; i++)
            {
                if (forbiddenChars.Contains(param[i]))
                    sb.Append(' ');
                else
                    sb.Append(param[i]);

            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a bitmap image object based on file.
        /// </summary>
        /// <remarks>
        /// Created image is a snapshot of file state. After creating BitmapImage
        /// file is open for all I/O operations, thus allowing it to be overwritten later.
        /// </remarks>
        private BitmapImage CreateBitmapFromFile(Uri uriSource)
        {
            // this actually took me a while to figure out :)
            BitmapImage result = new BitmapImage();
            result.BeginInit();

            // When creating bitmap ignore the cached image, and read from file instead
            result.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

            // the image is loaded to cache, instantly on creation to prevent locking file (we overwrite it later)
            result.CacheOption = BitmapCacheOption.OnLoad;

            result.UriSource = uriSource;
            result.EndInit();
            
            result.Freeze();
            return result;
        }

        /// <summary>
        /// Creates BitmapImage object based on a given Uri.
        /// </summary>
        /// <remarks>If Uri doesn't contain JPEG file, creation fails and null value is returned.</remarks>
        private BitmapImage CreateBitmapFromWebUrl(Uri url)
        {
            WebRequest request = WebRequest.CreateHttp(url);
            request.Timeout = 10000;

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException)
            {
                return null;
            }
            BitmapImage result = new BitmapImage();

            using (Stream stream = response.GetResponseStream())
            using (MemoryStream memory = new MemoryStream())
            {
                try
                {
                    stream.CopyTo(memory);
                    stream.Close();
                    memory.Position = 0;

                    result.BeginInit();
                    result.StreamSource = memory;
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.EndInit();

                    result.Freeze();
                    memory.Close();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return result;
        }

        #endregion
    }
}
