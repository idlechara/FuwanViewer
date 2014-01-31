using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Fuwanovel
{
    /// <summary>
    /// Represents a Visual Novel retrieved from Fuwanovel API.
    /// </summary>
    public class FuwaVisualNovel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string company { get; set; }
        public string homepage { get; set; }
        
        public List<string> tags { get; set; }

        public Texts texts { get; set; }

        public Images images { get; set; }

        public Dates dates { get; set; }

        public Site_Flags site_flags { get; set; }

        public Nullable<Torrent> torrent { get; set; }

        public Tl_Flags tl_flags { get; set; }

        public Egs egs { get; set; }

        public Vntls vntls { get; set; }

        public Links links { get; set; }
    }

    #region Child structures

    public struct Texts
    {
        public string description { get; set; }
        public string summary { get; set; }
        public string team { get; set; }
        public string notes { get; set; }
        public string troubleshoot { get; set; }
    }

    public struct Images
    {
        public string banner { get; set; }
        public string browse { get; set; }
        public string featured { get; set; }
        public string frontpage { get; set; }
        public string main { get; set; }
        public List<Screenshoot> screenshots { get; set; }
        public string Upcoming { get; set; }
    }

    public struct Screenshoot
    {
        public string full { get; set; }
        public string thumb { get; set; }
    }

    public struct Dates
    {
        public Nullable<DateTime> released { get; set; }
        public Nullable<DateTime> translated { get; set; }
        public Nullable<DateTime> added { get; set; }
    }

    public struct Torrent
    {
        public int id { get; set; }
        public string name { get; set; }
        public int size /* kB */ { get; set; }
        public Requirements requirements { get; set; }
        public string url { get; set; }
    }

    public struct Requirements
	{
        public bool install { get; set; }
        public bool jp_locale { get; set; }
        public bool patch { get; set; }
	}

    public struct Tl_Flags
    {
        public bool otome { get; set; }
        public bool sex { get; set; }
    }

    public struct Egs
    {
        public int score { get; set; }
        public int votes { get; set; }
    }

    public struct Vntls
    {
        public string alias { get; set; }
        public int progress { get; set; }
        public string status { get; set; }
    }

    public struct Links
    {
        public string faq { get; set; }
        public string forum { get; set; }
        public string op { get; set; }
        public string vndb { get; set; }
        public string vntls { get; set; }
        public string egs { get; set; }
        public string walkthrough { get; set; }
    }

    public struct Site_Flags
	{
        public bool released { get; set; }
        public bool active { get; set; }
        public bool featured { get; set; }
	}

    #endregion // child structures
}
