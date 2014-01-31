using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Fuwanovel;
using FuwanViewer.Model.Infrastructure;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository.Proxy;

namespace FuwanViewer.Repository.Cache
{
    /// <summary>
    /// Represents an Cache for VisualNovels returned from FuwanovelAPI.
    /// </summary>
    /// <remarks>
    /// This cache interprets object as fresh for at least 'CacheDuration', however
    /// after that expires a quick request is sent to Fuwanovel to check if object
    /// was actually modified. If not then object is "refreshed", i.e. stays fresh for another
    /// period of 'CacheDuration'.
    /// </remarks>
    [DataContract]
    public class FuwanovelCache : ICache<int, FuwaVisualNovel>
    {
        [DataMember]
        private FuwaVNRepository _parentRepository;
        [DataMember]
        private Dictionary<int, Entry> _entries = new Dictionary<int, Entry>();
        [DataMember]
        private DateTime _lastSynchronization;

        [DataMember]
        public TimeSpan CacheDuration { get; set; }

        #region Constructors

        public FuwanovelCache(FuwaVNRepository parentRepository) : this(parentRepository, TimeSpan.FromMinutes(10)) { }

        public FuwanovelCache(FuwaVNRepository parentRepository, TimeSpan cacheDuration)
        {
            _parentRepository = parentRepository;
            CacheDuration = cacheDuration;
        }

        #endregion // Constructors

        #region Public Interface

        /// <summary>
        /// Returns entry with given id, or null if
        /// entry is not valid anymore or doesn't exist.
        /// </summary>
        /// <remarks> Entry is valid if CacheDuration hasn't expired or API didn't change since entry was created.</remarks>
        public FuwaVisualNovel Retrieve(int id)
        {
            if (_entries.ContainsKey(id) && IsValid(_entries[id]))
                return _entries[id].FuwaVn;
            else
                return null;
        }

        /// <summary>
        /// Returns list of cached visual novels or null, if MinFreshness time has expired and API changed since last synchronization.
        /// </summary>
        public IEnumerable<FuwaVisualNovel> RetrieveAll()
        {
            if (_lastSynchronization == default(DateTime))
                return null;

            if (DateTime.Now - _lastSynchronization < CacheDuration)
                return _entries.Values.Select((entry) => entry.FuwaVn);
            else
            {
                var lastModified = FuwanovelAPI.GetLastModified();
                if (_lastSynchronization > lastModified)
                {
                    _lastSynchronization = DateTime.Now;
                    return _entries.Values.Select((entry) => entry.FuwaVn);
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Stores an object in the cache.
        /// </summary>
        public void Store(int id, FuwaVisualNovel fuwaVn)
        {
            if (_entries.ContainsKey(id))
                _entries[id] = new Entry(fuwaVn);
            else
                _entries.Add(id, new Entry(fuwaVn));
        }

        /// <summary>
        /// Syncronizes cache with given collection.
        /// </summary>
        public void Synchronize(IEnumerable<FuwaVisualNovel> collection)
        {
            Clear();
            foreach (var item in collection)
            {
                Store(item.id, item);
            }
            _lastSynchronization = DateTime.Now;
        }

        public void Remove(int id)
        {
            FuwaVisualNovel fuwaVn = _entries[id].FuwaVn;

            // RemoveImages(fuwaVn);
            _entries.Remove(id);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        #endregion // Public Interface

        #region Helper functions

        /// <summary>
        /// Checks wheter given entry is valid.
        /// </summary>
        /// <returns>True if CacheDuration hasn't expired or API didn't change since entry was created. False otherwise.</returns>
        /// <remarks>If Api fails for any reason, entry is assumed to be invalid.</remarks>
        private bool IsValid(Entry entry)
        {
            if (DateTime.Now - entry.Timestamp < CacheDuration)
                return true;
            else
            {
                var lastApiChange = FuwanovelAPI.GetLastModifiedFor(entry.FuwaVn.id);
                if (entry.Timestamp > lastApiChange)
                {
                    entry.Timestamp = DateTime.Now;
                    return true;
                }
                else
                    return false;
            }
        }
        
        /// <summary>
        /// Removes entry's images from corresponding ImageCache.
        /// </summary>
        private void RemoveImages(FuwaVisualNovel fuwaVn)
        {
            var imageCache = _parentRepository._imageCache;

            if (fuwaVn.images.main != null)
                imageCache.Remove(new Uri(fuwaVn.images.main));
            if (fuwaVn.images.browse != null)
                imageCache.Remove(new Uri(fuwaVn.images.browse));
            foreach (string ssUrl in fuwaVn.images.screenshots.ConvertAll(fuwaSs => fuwaSs.full))
            {
                if (ssUrl != null)
                    imageCache.Remove(new Uri(ssUrl));
            }
        }

        #endregion // Helper functions

        [DataContract]
        private struct Entry
        {
            [DataMember]
            public FuwaVisualNovel FuwaVn;
            [DataMember]
            public DateTime Timestamp;

            public Entry(FuwaVisualNovel fuwaVn) : this(fuwaVn, DateTime.Now) { }

            public Entry(FuwaVisualNovel fuwaVn, DateTime timestamp)
            {
                FuwaVn = fuwaVn;
                Timestamp = timestamp;
            }
        }
    }
}
