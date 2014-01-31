using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FuwanViewer.Model;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Presentation.ViewModels.Abstract
{
    /// <summary>
    /// Base class for all SearchGroups' ViewModels
    /// </summary>
    public abstract class SearchGroupViewModel : ViewModelBase
    {
        /// <summary>
        /// Condition defined by this SearchGroup that must be 
        /// satisfied by search results
        /// </summary>
        abstract public Predicate<VisualNovel> SearchPredicate { get; }
    }
}
