using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Presentation.ViewModels.SearchGroups;
using FuwanViewer.Services;
using FuwanViewer.Services.Interfaces;

namespace FuwanViewer.Presentation.ViewModels
{

    /// <summary>
    /// View model that operates on collection view and exposes interface
    /// to filter / search the collection.
    /// </summary>
    public class SearchPanelViewModel : SidePanelViewModel
    {
        #region Fields and Properties

        public ICommand SearchCommand { get; private set; }
        public List<SearchGroupViewModel> SearchGroups { get; set; }
        public ICollectionView Context { get; set; }

        #endregion // Fields and Properties

        #region Constructors

        public SearchPanelViewModel()
        {
            base.DisplayName = "Search panel";
            
            this.SearchCommand = new RelayCommand(Search);

            this.SearchGroups = new List<SearchGroupViewModel>();
            SearchGroups.Add(new TitleSearchGroupViewModel());
            SearchGroups.Add(new CompanySearchGroupViewModel());
            SearchGroups.Add(new TagSearchGroupViewModel());
            SearchGroups.Add(new OtherSearchGroupViewModel());
        }

        #endregion // Constructors

        void Search(object param)
        {
            if (Context == null)
                return;

            // Create filter by combining predicates
            Context.Filter = delegate(object item)
            {
                VisualNovel vn = item as VisualNovel;
                bool result = true;

                foreach (var searchGroup in SearchGroups)
                {
                    result = result && searchGroup.SearchPredicate(vn);
                }
                return result;
            };
            
            // Refresh collectionView
            Context.Refresh();
        }
    }
}
