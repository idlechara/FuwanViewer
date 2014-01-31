using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Model.Infrastructure
{
    /// <summary>
    /// Exposes properties to control cache based on FileSystem and
    /// implements IDisposable interface.
    /// </summary>
    public interface IFileCache<TId, T> : ICache<TId, T>, IDisposable
    {
        /// <summary>
        /// Represents wheter cache size is limited or not.
        /// </summary>
        bool LimitSize { get; set; }
        
        /// <summary>
        /// Represents maximum size that this cache can reach.
        /// </summary>
        /// <remarks>This has no effect if LimitSize is set to False</remarks>
        long MaximumSize { get; set; }

        /// <summary>
        /// Represents a Directory path where files are being stored.
        /// </summary>
        string Directory { get; set; }
    }
}
