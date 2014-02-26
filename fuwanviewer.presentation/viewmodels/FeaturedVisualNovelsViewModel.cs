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
    public class FeaturedVisualNovelsViewModel : WorkspaceViewModel
    {
        #region Fields and Properties

        IVisualNovelService _vnService;
        Action<VisualNovel> _openVisualNovel;
        List<VisualNovelWidgetViewModel> _visualNovels { get; set; }

        public ICollectionView FeaturedNovels { get; private set; }

        #endregion // Fields and Properties

        #region Constructors

        public FeaturedVisualNovelsViewModel(Action<VisualNovel> openVisualNovel)
            : this(openVisualNovel, Application.Current.Properties["VisualNovelService"] as IVisualNovelService) 
        { }

        public FeaturedVisualNovelsViewModel(Action<VisualNovel> openVisualNovel, IVisualNovelService vnService)
        {
            // initialize members
            base.DisplayName = "Featured";
            this._visualNovels = new List<VisualNovelWidgetViewModel>();
            this._openVisualNovel = openVisualNovel;
            this._vnService = vnService;
            this.FeaturedNovels = CollectionViewSource.GetDefaultView(_visualNovels);

            // Refresh to get Visual Novels
            this.Refresh();
        }
        
        #endregion // Constructors

        override public async void Refresh()
        {
            UiServices.SetBusyState();

            var freshNovels = await Task<List<VisualNovel>>.Run(() => _vnService.GetFeaturedNovels());
            _visualNovels.Clear();
            _visualNovels.AddRange(freshNovels.ToList().ConvertAll(
                delegate (VisualNovel vn) 
                {
                    return new VisualNovelWidgetViewModel(vn, _openVisualNovel);
                }));
            FeaturedNovels.Refresh();

            UiServices.ClearBusyState();
        }
    }
}
