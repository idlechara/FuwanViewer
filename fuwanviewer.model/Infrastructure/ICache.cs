using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Model.Infrastructure
{
    /// <summary>
    /// Defines methods to manipulate generic Cache.
    /// </summary>
    /// <typeparam name="TId">Type of entry's Id value</typeparam>
    /// <typeparam name="T">Type of entries.</typeparam>
    public interface ICache<TId, T>
    {
        TimeSpan CacheDuration { get; set; }

        void Store(TId id, T entry);
        
        T Retrieve(TId id);
        IEnumerable<T> RetrieveAll();

        void Remove(TId id);
        void Clear();
    }
}
