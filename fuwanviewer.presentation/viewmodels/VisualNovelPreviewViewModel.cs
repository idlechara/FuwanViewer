using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FuwanViewer.Model;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.ViewModels
{
    public class VisualNovelPreviewViewModel : ViewModelBase
    {
        #region Fields and Properties

        private VisualNovel _novel;

        public string Title { get { return _novel != null ? _novel.Title : null; } }
        public string Description { get { return _novel != null ? _novel.Description : null; } }
        public List<Tag> Tags { get { return _novel != null ? _novel.Tags : null; } }
        public BitmapImage Cover { get { return _novel != null ? _novel.Cover : null; } }
        public BitmapImage MainArt { get { return _novel != null ? _novel.MainArt : null; } }

        #endregion // Fields and Properties

        #region Constructors

        public VisualNovelPreviewViewModel(VisualNovel vn)
        {
            SetVisualNovel(vn);
        }

        #endregion // Constructors

        #region Public Interface 

        public void SetVisualNovel(VisualNovel vn)
        {
            if (_novel != null)
                _novel.PropertyChanged -= novel_PropertyChanged;

            _novel = vn;

            if (_novel != null)
                this._novel.PropertyChanged += novel_PropertyChanged;

            // this is necessary to force bindings to update
            OnPropertyChanged("Title");
            OnPropertyChanged("Description");
            OnPropertyChanged("Cover");
            OnPropertyChanged("MainArt");
            OnPropertyChanged("Tags");
        }

        #endregion // Public Interface 

        // relay VisualNovels PropertyChanged to ViewModel property Changed
        void novel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}
