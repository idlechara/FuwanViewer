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
    public class VisualNovelWidgetViewModel : ViewModelBase
    {
        #region Fields and Properties

        internal VisualNovel _novel;

        public string Title { get { return _novel != null ? _novel.Title : null; } }
        public string Description { get { return _novel != null ? _novel.Description : null; } }
        public List<Tag> Tags { get { return _novel != null ? _novel.Tags : null; } }
        public BitmapImage MainArt { get { return _novel != null ? _novel.MainArt : null; } }

        public bool IsNull 
        {
            get { return _novel == null; }
        }

        public ICommand OpenVisualNovelCommand { get; set; }
        public ICommand RefreshVisualNovelCommand { get; set; }
        public ICommand HandleRequestNavigateCommand { get; set; }
        public ICommand ImageLoadedCommand { get; set; }

        #endregion // Fields and Properties

        #region Constructors

        public VisualNovelWidgetViewModel(VisualNovel vn, Action<VisualNovel> openVisualNovel)
        {
            SetVisualNovel(vn);

            this.RefreshVisualNovelCommand = new RelayCommand(RefreshVisualNovel);
            this.ImageLoadedCommand = new RelayCommand(ImageLoaded);
            this.OpenVisualNovelCommand = new RelayCommand(() => openVisualNovel(_novel));
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

        private void RefreshVisualNovel(object param)
        {
            var vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;
            SetVisualNovel(vnService.GetVisualNovel(_novel.Id));
        }

        void ImageLoaded(object param)
        {
        }
        #endregion // Commands

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
