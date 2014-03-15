using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Services;
using FuwanViewer.Services.Interfaces;
using FuwanViewer.Presentation.EventBehaviours;
using System.Windows.Media.Animation;

namespace FuwanViewer.Presentation.ViewModels
{
    public class VisualNovelViewModel : WorkspaceViewModel
    {
        #region Fields and Properties

        internal VisualNovel _novel;
        internal BitmapImage _current_screnshot;

        // there was some weird bug due to the way WPF's tab control works,
        // end even though, Cover and such properties, shoudln't be ever called when novel == null,
        // because Template should change (I did set data trigger to make it so)
        // so that they are not displayed... that was not the case :(
        public string Title { get { return _novel != null ? _novel.Title : null; } }
        public string Description { get { return _novel != null ? _novel.Description : null; } }

        //Kuky-code begin
        public string Company { get { return _novel != null ? _novel.Company : null; } }
        //idk if the release date is null, so i'll asume that is always present.
        public string ReleaseDate { get { return _novel != null ? _novel.ReleaseDate.ToShortDateString() : null; } }
        public List<Tag> Tags { get { return _novel != null ? _novel.Tags : null; } }
        public string Summary { get { return _novel != null ? _novel.Summary : null; } }

        //TODO TEAM and Translation
        public string TeamListing { get { return _novel != null ? _novel.TeamListing : null; } }
        public string Notes { get { return _novel != null ? _novel.Notes : null; } }
        public string TroubleshootText { get { return _novel != null ? _novel.TroubleshootText : null; } }

        //TODO EGS
        public string  EgsScore { get { return _novel != null ? _novel.EgsScore.ToString(): null; } }
        public string EgsVotes { get { return _novel != null ? _novel.EgsVotes.ToString() : null; } }

        //TODO GuestComments

        //TODO VisualNovelInstallationDataAndTorrent

        //TODO links
        public string FuwanovelUrl { get { return _novel != null ? _novel.FuwanovelUrl: null; } }
        public string TranslationHomepageUrl { get { return _novel != null ? _novel.TranslationHomepageUrl : null; } }
        public string FaqUrl { get { return _novel != null ? _novel.FaqUrl : null; } }
        public string ForumUrl { get { return _novel != null ? _novel.ForumUrl : null; } }
        public string WalkthroughUrl { get { return _novel != null ? _novel.WalkthroughUrl : null; } }

        public bool WalkthroughUrlExists { get { return _novel != null && _novel.WalkthroughUrl!= null ? true : false; } }
        public bool ForumUrlExists { get { return _novel != null && _novel.ForumUrl != null ? true : false; } }
        public bool FaqUrlExists { get { return _novel != null && _novel.FaqUrl != null ? true : false; } }



        public string TorrentURL { get { return _novel != null && _novel.Torrent != null ? _novel.Torrent.Url : null; } }
        public bool TorrentRequiresInstall { get { return _novel != null && _novel.Torrent != null ? _novel.Torrent.RequiresInstall : default(bool); } }
        public bool TorrentRequiresJpLocale { get { return _novel != null && _novel.Torrent != null ? _novel.Torrent.RequiresJpLocale : default(bool); } }
        public int TorrentSize { get { return _novel != null && _novel.Torrent != null ? _novel.Torrent.Size : default(int); } }
        public string TorrentName { get { return _novel != null && _novel.Torrent != null ? _novel.Torrent.Name : null; } }

        public bool TorrentExists { get { return _novel != null && _novel.Torrent != null ? true : false; } }

        //fix for screenshots

        //Kuky-code end

        public BitmapImage MainArt { get { return _novel != null ? _novel.MainArt : null; } }

        public BitmapImage CurrentScreenshot { 
            get{
                return _current_screnshot;
            }
            set {
                _current_screnshot = value;
                OnPropertyChanged("CurrentScreenshot");
                OnPropertyChanged("ScreenshotModeEnabled");
            } 
        }

        public Boolean ScreenshotModeEnabled { 
            get 
            { 
                return CurrentScreenshot != null ? true : false; 
            } 
        }
        public ICollection<BitmapImage> Screenshoots { get { return _novel != null ? _novel.Screenshoots : null; } }

        public override string DisplayName { get { return Title; } }
        public bool IsNull 
        {
            get { return _novel == null; }
        }

        public ICommand RefreshVisualNovelCommand { get; set; }
        public ICommand HandleRequestNavigateCommand { get; set; }
        public ICommand ImageLoadedCommand { get; set; }
        public ICommand ToggleScreenshotViewCommand { get; set; }

        #endregion // Fields and Properties

        #region Constructors

        public VisualNovelViewModel(VisualNovel vn)
        {
            SetVisualNovel(vn);

            this.RefreshVisualNovelCommand = new RelayCommand(RefreshVisualNovel);
            this.HandleRequestNavigateCommand = new RelayCommand(HandleRequestNavigate);
            this.ImageLoadedCommand = new RelayCommand(ImageLoaded);
            this.ToggleScreenshotViewCommand = new RelayCommand(ToggleScreenshotView);
        }

        #endregion // Constructors

        #region Event Handlers

        void vn_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // relay VisualNovels PropertyChanged to ViewModel property Changed
            OnPropertyChanged(e.PropertyName);
        } 


        #endregion // Event Handlers

        #region Commands

        private void ToggleScreenshotView(object param)
        {
            if (CurrentScreenshot == null)
            {
                BitmapImage parameter = (BitmapImage)param;
                CurrentScreenshot = parameter;
            }
            else
            {
                CurrentScreenshot = null;
            }
        }


        private void RefreshVisualNovel(object param)
        {
            var vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;
            SetVisualNovel(vnService.GetVisualNovel(_novel.Id));
        }


        // Taken from http://laurenlavoie.com/avalon/159
        // used to lauch a new browser tab/window from the hiperlink
        void HandleRequestNavigate(object param)
        {
            string navigateUri = (string)param;

            if (navigateUri == default(string))
                return;

            // if the URI somehow came from an untrusted source, make sure to
            // validate it before calling Process.Start(), e.g. check to see
            // the scheme is HTTP, etc.
            Process.Start(new ProcessStartInfo(navigateUri));
        }

        void ImageLoaded(object param)
        {
            MessageBox.Show("images: "  + Screenshoots.Count.ToString());
        }
        #endregion // Commands

        override public async void Refresh()
        {
            if (_novel == null)
                return;

            UiServices.SetBusyState();

            var vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;
            var freshNovel = await Task<VisualNovel>.Run(() => vnService.GetVisualNovel(_novel.Id));
            SetVisualNovel(freshNovel);

            UiServices.ClearBusyState();
        }

        #region Helper functions

        private void SetVisualNovel(VisualNovel newNovel)
        {
            // unsubscribe from old novel's events
            if (this._novel != null)
            {
                this._novel.PropertyChanged -= vn_OnPropertyChanged;
            }

            // assign new novel
            this._novel = newNovel;

            // if not null, relay propertychanged events,
            if (this._novel != null)
            {
                _novel.PropertyChanged += vn_OnPropertyChanged;
            }

            // force bindings to update
            Type t = typeof(VisualNovelViewModel);
            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                OnPropertyChanged(property.Name);
            }
        }

        #endregion // Helper functions
    }
}
