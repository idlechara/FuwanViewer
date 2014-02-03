using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Repository;
using FuwanViewer.Model;
using FuwanViewer.Presentation.ViewModels.Abstract;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FuwanViewer.Services;
using FuwanViewer.Model.Users;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Services.Interfaces;

namespace FuwanViewer.Presentation.ViewModels
{
    public class AllVisualNovelsViewModel : WorkspaceViewModel
    {
        #region Fields and Properties

        VisualNovelPreviewViewModel _visualNovelPreview;
        List<VisualNovel> _visualNovels;
        ICollectionView _allNovels;

        public ICommand OpenVisualNovelCommand { get; private set; }

        public ICollectionView AllNovels 
        {
            get { return _allNovels; }
        }
        public VisualNovelPreviewViewModel VisualNovelPreview 
        { 
            get {return _visualNovelPreview;}
            set 
            {
                _visualNovelPreview = value;
                OnPropertyChanged();
            } 
        }
        public User User { get; set; }

        #endregion // Fields and Properties

        #region Constructors

        public AllVisualNovelsViewModel(Action<VisualNovel> openVisualNovel)
            : this(openVisualNovel, 
                   Application.Current.Properties["VisualNovelService"] as IVisualNovelService, 
                   null) { }

        public AllVisualNovelsViewModel(Action<VisualNovel> openVisualNovel, VisualNovelService vnService)
            : this(openVisualNovel,
                   vnService,
                    null) { }

        public AllVisualNovelsViewModel(Action<VisualNovel> openVisualNovel, 
                                        IVisualNovelService vnService, 
                                        User user)
        {
            // initialize members
            base.DisplayName = "All novels";
            this._visualNovels = new List<VisualNovel>();
            this._allNovels = CollectionViewSource.GetDefaultView(_visualNovels);
            this._visualNovelPreview = new VisualNovelPreviewViewModel(AllNovels.CurrentItem as VisualNovel);
            this.OpenVisualNovelCommand = new RelayCommand((param) => 
                {
                    if (param != null)
                        openVisualNovel(param as VisualNovel);   
                });

            this._allNovels.CurrentChanged += SetPreviewNovel;
            this.Refresh();
        }
        
        #endregion // Constructors

        void SetPreviewNovel(object sender, EventArgs e)
        {
            // update visual novel preview
            ICollectionView collectionView = sender as ICollectionView;
            VisualNovel vn = collectionView.CurrentItem as VisualNovel;

            VisualNovelPreview.SetVisualNovel(vn);
        }

        override public async void Refresh()
        {
            UiServices.SetBusyState();

            var vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;
            var freshNovels = await Task<List<VisualNovel>>.Run(() => vnService.GetAllVisualNovels());
            _visualNovels.Clear();
            _visualNovels.AddRange(freshNovels);
            AllNovels.Refresh();

            UiServices.ClearBusyState();
        }
    }
}
