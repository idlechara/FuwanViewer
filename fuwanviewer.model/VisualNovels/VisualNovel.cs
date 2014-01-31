using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Media.Imaging;

namespace FuwanViewer.Model.VisualNovels
{
    /// <summary>
    /// Class representing a visual novel.
    /// </summary>
    /// <remarks>
    /// This class implements INotifyPropertyChanged interface and
    /// supports DataContract serialization.
    /// </remarks>
    [DataContract]
    public abstract class VisualNovel : INotifyPropertyChanged
    {
        #region Fields

        string _summary;
        string _description;

        #endregion // Fields

        #region Properties

        //Fuwanovel id of this VisualNovel
        [DataMember]
        public int Id { get; set; }

        // Basic informations
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public List<Tag> Tags { get; set; }
        [DataMember]
        public bool SexContent { get; set; }
        [DataMember]
        public bool Otome { get; set; }

        //EGS
        [DataMember]
        public int EgsScore { get; set; }
        [DataMember]
        public int EgsVotes { get; set; }

        // Dates
        [DataMember]
        public DateTime ReleaseDate { get; set; }
        [DataMember]
        public DateTime TranslateDate { get; set; }

        // Flags
        [DataMember]
        public bool IsFeatured { get; set; }
        public bool Translated 
        {
            get { return TranslateDate != default(DateTime); }
        }
        public bool Released
        {
            get { return ReleaseDate != default(DateTime); }
        }

        // Texts
        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        [DataMember]
        public string Summary 
        { 
            get { return _summary; } 
            set { _summary = value; OnPropertyChanged(); } 
        }
        [DataMember]
        public string TeamListing { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string TroubleshootText { get; set; }

        // Links
        [DataMember]
        public string FuwanovelUrl { get; set; }
        [DataMember]
        public string TranslationHomepageUrl { get; set; }
        [DataMember]
        public string FaqUrl { get; set; }
        [DataMember]
        public string ForumUrl { get; set; }
        [DataMember]
        public string WalkthroughUrl { get; set; }

        // Torrent
        [DataMember]
        public Torrent Torrent { get; set; }

        // Images
        public virtual BitmapImage Cover { get; set; }
        public virtual BitmapImage MainArt { get; set; }
        public virtual ICollection<BitmapImage> Screenshoots { get; set; }


        #endregion // Properties

        #region Constructors

        public VisualNovel() { }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// Refreshes BitmapImages in visual novel;
        /// </summary>
        public virtual void Refresh() { }

        /// <summary>
        /// Compares visual novels by ID and Title
        /// </summary>
        /// <returns>True if novels have the same id.</returns>
        public override bool Equals(object obj)
        {
            var vn = obj as VisualNovel;
            
            if (vn == null)
                return false;
            else
                return this.Id == vn.Id;
        }

        #endregion // Methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion  // INotifyPropertyChanged
    }
}
