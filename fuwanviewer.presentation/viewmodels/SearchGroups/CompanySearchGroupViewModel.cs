using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.ViewModels.SearchGroups
{
    public class CompanySearchGroupViewModel : SearchGroupViewModel
    {
        public string SearchString { get; set; }

        public override Predicate<VisualNovel> SearchPredicate
        {
            get
            {
                return (vn) => vn.Company.Contains(SearchString);
            }
        }

        #region Constructors

        public CompanySearchGroupViewModel()
        {
            base.DisplayName = "Company";
            this.SearchString = string.Empty;
        }

        #endregion // Constructors
    }
}
