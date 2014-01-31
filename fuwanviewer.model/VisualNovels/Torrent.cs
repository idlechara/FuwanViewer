using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Model.VisualNovels
{
    [DataContract]
    public class Torrent
    {
        // basic informations
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public string Url { get; set; }

        // requirements
        [DataMember]
        public bool RequiresInstall { get; set; }
        [DataMember]
        public bool RequiresJpLocale { get; set; }
        [DataMember]
        public bool Patch { get; set; }

        public Torrent() { }
    }
}
