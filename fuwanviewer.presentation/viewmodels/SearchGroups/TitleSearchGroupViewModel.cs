using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Presentation.ViewModels.SearchGroups
{
    public class TitleSearchGroupViewModel : SearchGroupViewModel
    {
        public string SearchString { get; set; }

        public override Predicate<VisualNovel> SearchPredicate
        {
            get
            {
                return (vn) => vn.Title.Contains(SearchString);
            }
        }

        #region Constructors

        public TitleSearchGroupViewModel()
        {
            base.DisplayName = "Title";
            this.SearchString = string.Empty;
        }

        #endregion // Constructors
    }
}
