using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Repository.Cache
{
    /// <summary>
    /// Represents an Image Cache, that doesn't cache anything.
    /// </summary>
    [DataContract]
    public class NullImageCache : ICache<Uri, BitmapImage>
    {
        public TimeSpan CacheDuration { get {return TimeSpan.Zero;} set {} }

        public BitmapImage Retrieve(Uri key)
        {
            return null;
        }

        public IEnumerable<BitmapImage> RetrieveAll()
        {
            return new List<BitmapImage>();
        }

        public void Store(Uri key, BitmapImage entry)
        {
        }

        public void Remove(Uri key)
        {
        }

        public void Clear() { }
    }
}
