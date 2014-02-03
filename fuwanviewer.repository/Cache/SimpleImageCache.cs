using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Fuwanovel;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Repository.Cache
{
    /// <summary>
    /// Represents a Cache for images based on filesystem.
    /// </summary>
    /// <remarks>
    /// Images are assumed to be valid for at least 'CacheDuration', after which
    /// a web request to corresponding url (key of this cache) is sent to check
    /// if image was modified. If NOT then entry stays valid for another 14 days.
    /// </remarks>
    [DataContract]
    [KnownType(typeof(EntryInfo))]
    public class SimpleImageCache : IFileCache<Uri, BitmapImage>
    {
        #region Fields and Properties

        [DataMember]
        private Dictionary<Uri, EntryInfo> _entriesData = new Dictionary<Uri,EntryInfo>();
        [DataMember]
        private string _imageDirectory;

        #endregion // Fields and Properties

        #region IFileCache

        [DataMember]
        public TimeSpan CacheDuration { get; set; }

        [DataMember]
        public bool LimitSize { get; set; }

        [DataMember]
        public long MaximumSize { get; set; }

        public string Directory
        {
            get
            {
                return _imageDirectory;
            }
            set
            {
                // if same value, return
                if (value == _imageDirectory)
                    return;

                // clear cache
                Clear();

                // delete folder if empty
                if (System.IO.Directory.Exists(_imageDirectory) && System.IO.Directory.GetFileSystemEntries(_imageDirectory).Length == 0)
                    System.IO.Directory.Delete(_imageDirectory);

                _imageDirectory = value;
                CreateImageDirectory();
            }
        }

        public long CurrentSize
        {
            get
            {
                return CalculateCacheSize();
            }
        }

        #endregion // Fields and Properties

        #region Constructors

        public SimpleImageCache() : this(TimeSpan.FromDays(14)) { }

        public SimpleImageCache(TimeSpan cacheDuration)
        {
            CacheDuration = cacheDuration;
        }

        #endregion // Constructors

        #region ICache

        public BitmapImage Retrieve(Uri key)
        {
            if (_entriesData.ContainsKey(key) == false)
                return null;
            else if (IsValid(key) == false)
                return null;
            else
            {
                // update 'last use' and return BitmapImage
                EntryInfo resultEntry = _entriesData[key];
                _entriesData[key] = new EntryInfo(resultEntry.timestamp, DateTime.Now, resultEntry.file.FullName);
                return CreateBitmapImage(resultEntry.file);
            }
        }

        public IEnumerable<BitmapImage> RetrieveAll()
        {
            throw new NotSupportedException();
        }

        public void Store(Uri key, BitmapImage image)
        {
            FileInfo fi = new FileInfo(GenerateFileName(key));

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            // save / overwrite file
            if (System.IO.Directory.Exists(Directory) == false)
                CreateImageDirectory();

            string filename = GenerateFileName(key);
            FileStream fStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            encoder.Save(fStream);
            fStream.Close();

            // add / update entryInfo
            if (_entriesData.ContainsKey(key))
            {
                _entriesData[key] = new EntryInfo(DateTime.Now, DateTime.Now, _entriesData[key].file.FullName);
            }
            else
            {
                _entriesData.Add(key, new EntryInfo(DateTime.Now, DateTime.Now, filename));
            }

            // if cache got to big => clean up
            if (LimitSize == true && CurrentSize > MaximumSize)
            {
                CleanUp();
            }
        }

        public void Remove(Uri key)
        {
            if (_entriesData.ContainsKey(key))
            {
                EntryInfo entryInfo = _entriesData[key];

                // remove file
                if (entryInfo.file.Exists == true)
                    File.Delete(entryInfo.file.FullName);

                // remove entry info
                _entriesData.Remove(key);
            }
        }

        public void Clear()
        {
            var keys = _entriesData.Keys.ToList();
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        #endregion // ICache

        #region IDisposable

        /// <summary>
        /// Deletes all the files associated with cache, including file directory
        /// if it ends up empty.
        /// </summary>
        public void Dispose()
        {
            Clear();

            if (System.IO.Directory.Exists(_imageDirectory) && System.IO.Directory.GetFileSystemEntries(_imageDirectory).Length == 0)
                System.IO.Directory.Delete(_imageDirectory);
        }

        #endregion // IDisposable

        #region Helper functions

        /// <summary>
        /// Assumes the images to be valid for 14 days, after that it
        /// checkes wheter image was modified, if not image stays valid for another
        /// 14 days.
        /// </summary>
        /// <returns>True if entry is valid, false otherwise.</returns>
        private bool IsValid(Uri key)
        {
            EntryInfo entry = _entriesData[key];

            // if file disappeared => False
            if (entry.file.Exists == false)
                return false;

            // if timestamp is less than 'CacheDuration' days from now => True
            if (DateTime.Now - entry.timestamp < CacheDuration)
                return true;

            // Request last modified from Fuwanovel, return true if NOT modified.
            if (key.Scheme != Uri.UriSchemeHttp)
                return false;

            HttpWebRequest request = WebRequest.CreateHttp(key);
            request.Method = "HEAD";
            request.Timeout = 5000;
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (entry.timestamp >= response.LastModified)
                    {
                        _entriesData[key] = new EntryInfo(DateTime.Now, entry.lastUse, entry.file.FullName);
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        private string GenerateFileName(Uri key)
        {
            string absolutePath = key.AbsolutePath;

            int count = 0; int lastIndex = 0; int secondLastIndex= 0;
            for (int i = absolutePath.Length - 1; i > 0; i--)
            {
                if (absolutePath[i] == '/')
                {
                    count++;
                    if (count == 1)
                    {
                        lastIndex = i;
                    }
                    else if (count == 2)
                    {
                        secondLastIndex = i;
                        break;
                    }
                }
            }

            // return result (in form: "{imageDirectory}/{id}-{filename})
            StringBuilder result = new StringBuilder();
            result.Append(_imageDirectory).Append('\\').
                Append(absolutePath.Substring(secondLastIndex +1, lastIndex - secondLastIndex - 1)).
                Append('-').Append(absolutePath.Substring(lastIndex +1));
            result.Replace("%20", " ");

            return result.ToString();
        }

        private BitmapImage CreateBitmapImage(FileInfo fi)
        {
            BitmapImage result = new BitmapImage();

            result.BeginInit();
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            result.UriSource = new Uri(fi.FullName);
            result.EndInit();

            return result;
        }

        /// <summary>
        /// Performs a clean up on Cache entries in order to meet MaxSize constrain.
        /// </summary>
        /// <remarks>
        /// Entries are bein removed in order of last use (most recently used, are last)
        /// untill cache goes below 75% of maximum size constrain.
        /// </remarks>
        private void CleanUp()
        {
            // sort entries by LastUse (Least recently used first, Most recently used last)
            var entriesSortedByUse = _entriesData.ToList();    
            entriesSortedByUse.Sort((x, y) =>
            {
                if (x.Value.lastUse == y.Value.lastUse)
                    return 0;
                else if (x.Value.lastUse > y.Value.lastUse)
                    return 1;
                else
                    return -1;
            });

            // remove entries until cache reaches 3/4 of maximum size
            long removedBytes = 0; long initialSize = CurrentSize;
            for (int i = 0; i < entriesSortedByUse.Count; i++)
            {
                Uri keyToRemove = entriesSortedByUse[i].Key;
                removedBytes += entriesSortedByUse[i].Value.file.Length;
                Remove(keyToRemove);

                if (initialSize - removedBytes < 0.75 * MaximumSize)
                    break;
            }
        }

        private long CalculateCacheSize()
        {
            long result = 0;
            var keysToRemove = new List<Uri>();

            // add file sizes, collect entries without file
            foreach (var key in _entriesData.Keys)
            {
                EntryInfo entry = _entriesData[key];
                if (entry.file.Exists == true)
                    result += entry.file.Length;
                else
                    keysToRemove.Add(key);
            }
            // remove entries without file
            foreach (var key in keysToRemove)
            {
                Remove(key);
            }

            return result;
        }

        private void CreateImageDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(_imageDirectory);

            if (di.Exists == false)
                System.IO.Directory.CreateDirectory(_imageDirectory);
        }

        #endregion // Helper functions

        [DataContract]
        private struct EntryInfo
        {
            [DataMember]
            public DateTime timestamp;
            [DataMember]
            public DateTime lastUse;
            [DataMember]
            public FileInfo file;
            
            public EntryInfo(DateTime timestamp, DateTime lastUse, string filename)
            {
                this.timestamp = timestamp;
                this.lastUse = lastUse;
                this.file = new FileInfo(filename);
            }
        }
    }
}
