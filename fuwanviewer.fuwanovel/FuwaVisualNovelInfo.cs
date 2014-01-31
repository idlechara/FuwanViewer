using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Fuwanovel
{
    /// <summary>
    /// Represents basic information about VisualNovel from Fuwanovel API
    /// </summary>
    public class FuwaVisualNovelShort
    {
        /// <summary>Id</summary>
        public int id { get; set; }

        /// <summary>Title.</summary>
        public string name { get; set; }

        /// <summary>
        /// Link to this VisualNovel on Fuwanovel.org
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Link to this VisualNovel on FuwanovelAPI
        /// </summary>
        public string context { get; set; }
    }
}
