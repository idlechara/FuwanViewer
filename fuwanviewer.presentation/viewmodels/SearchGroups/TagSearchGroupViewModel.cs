using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Model;
using FuwanViewer.Model.VisualNovels;

namespace FuwanViewer.Presentation.ViewModels.SearchGroups
{
    public class TagSearchGroupViewModel : SearchGroupViewModel
    {
        public List<Pair<Tag, bool?>> Pairs {get; set;}

        public override Predicate<VisualNovel> SearchPredicate
        {
            get 
            { 
                return delegate(VisualNovel vn)
                {
                    foreach (var pair in Pairs)
                    {
                        if (pair.Item2 == null)
                        {
                            continue;
                        }
                        else if (pair.Item2 == true  && !vn.Tags.Contains(pair.Item1) ||
                                 pair.Item2 == false && vn.Tags.Contains(pair.Item1))
                        {
                            return false;
                        }
                    }
                    return true;
                };
            }
        }

        #region Constructors

        public TagSearchGroupViewModel()
        {
            base.DisplayName = "Tags";
            this.Pairs = new List<Pair<Tag,bool?>>() ;

            foreach (Tag tag in Enum.GetValues(typeof(Tag)).Cast<Tag>())
            {
                Pairs.Add(new Pair<Tag, bool?>(tag, null));
            }
        }

        #endregion // Constructors
    }
}
