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

        Action<VisualNovel> _openVisualNovel;
        public List<VisualNovelWidgetViewModel> FeaturedNovels { get; set; }
       
        #endregion // Fields and Properties

        #region Constructors

        public FeaturedVisualNovelsViewModel(Action<VisualNovel> openVisualNovel)
            : this(openVisualNovel, Application.Current.Properties["VisualNovelService"] as IVisualNovelService) 
        { }

        public FeaturedVisualNovelsViewModel(Action<VisualNovel> openVisualNovel, IVisualNovelService vnService)
        {
            // initialize members
            base.DisplayName = "Featured";
            this.FeaturedNovels = new List<VisualNovelWidgetViewModel>();
            this._openVisualNovel = openVisualNovel;

            // Refresh to get Visual Novels
            this.Refresh();
        }
        
        #endregion // Constructors

        override public async void Refresh()
        {
            UiServices.SetBusyState();

            var vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;
            var freshNovels = await Task<List<VisualNovel>>.Run(() => vnService.GetFeaturedNovels());
            FeaturedNovels.Clear();
            FeaturedNovels.AddRange(freshNovels.ToList().ConvertAll(
                delegate (VisualNovel vn) 
                {
                    return new VisualNovelWidgetViewModel(vn, _openVisualNovel);
                }));
            OnPropertyChanged("FeaturedNovels");

            UiServices.ClearBusyState();
        }
    }
}
