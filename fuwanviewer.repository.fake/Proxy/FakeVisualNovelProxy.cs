using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Runtime.Serialization;
using System.Reflection;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository.Proxy;
using FuwanViewer.Repository.Cache;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Repository.Fake.Proxy
{
    /// <summary>
    /// Fake class used to simulate long loading VisualNovels data from internet
    /// </summary>
    [DataContract]
    [KnownType(typeof(NullImageCache))]
    [KnownType(typeof(SimpleImageCache))]
    public class FakeVisualNovelProxy : VisualNovel
    {
        #region Fields and Properties

        private FakeFuwaVNRepository _parentRepository;

        [DataMember]
        private Dictionary<ResourceType, Uri> _resourceUris = new Dictionary<ResourceType, Uri>();
        
        private Dictionary<ResourceType, BitmapImage> _resourceImages = new Dictionary<ResourceType, BitmapImage>();
        private Dictionary<ResourceType, bool> _haveLoaded = new Dictionary<ResourceType, bool>();

        public override BitmapImage Cover
        {
            get
            {
                var resource = ResourceType.Cover;
                if (_haveLoaded[resource] == false)
                    LoadResource(resource);

                BitmapImage image = null;
                _resourceImages.TryGetValue(resource, out image);
                return image;
            }
        }
        public override BitmapImage MainArt
        {
            get
            {
                var resource = ResourceType.MainArt;
                if (_haveLoaded[resource] == false)
                    LoadResource(resource);

                BitmapImage image = null;
                _resourceImages.TryGetValue(resource, out image);
                return image;
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
                    BitmapImage image = null;
                    _resourceImages.TryGetValue(screenshoot, out image);
                    result.Add(image);
                }
                return result;
            }
        }

        private ICache<Uri, BitmapImage> ImageCache { get { return _parentRepository._imageCache; } }

        #endregion // Fields and Properties

        #region Constructor

        internal FakeVisualNovelProxy(FakeFuwaVNRepository parentRepo, Dictionary<ResourceType, Uri> uris)
        {
            _parentRepository = parentRepo;
            _resourceUris = uris;

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                _resourceImages.Add(resource, null);
                _haveLoaded.Add(resource, false);
            }
        }

        #endregion // Constructor

        private async void LoadResource(ResourceType resource)
        {
            _haveLoaded[resource] = true;

            // try retrieve from cache
            BitmapImage image = ImageCache.Retrieve(_resourceUris[resource]);
            // if no cache entry, simulate downloading
            if (image == null)
            {
                await Task.Run(() => { Thread.Sleep(500); });
            }
            _resourceImages[resource] = new BitmapImage(_resourceUris[resource]);
            ImageCache.Store(_resourceUris[resource], _resourceImages[resource]);
            OnPropertyChanged(GetPropertyName(resource));
        }

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

        #region Data Contract Serialization

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            _resourceImages = new Dictionary<ResourceType, BitmapImage>();
            _haveLoaded = new Dictionary<ResourceType, bool>();

            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
            {
                _resourceImages.Add(resource, null);
                _haveLoaded.Add(resource, false);
            }
        }

        #endregion // Data Contract Serialization
    }
}
