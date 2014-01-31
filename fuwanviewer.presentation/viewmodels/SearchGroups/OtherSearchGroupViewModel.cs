using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.ViewModels.SearchGroups
{
    public class OtherSearchGroupViewModel : SearchGroupViewModel
    {
        public bool? SexContent { get; set; }
        public bool? Otome { get; set; }
        public bool? Translated { get; set; }
        public bool? Released { get; set; }

        public OtherSearchGroupViewModel()
        {
            base.DisplayName = "Other";
            SexContent = null;
        }

        public override Predicate<VisualNovel> SearchPredicate
        {
            get 
            {
                return delegate(VisualNovel vn)
                {
                    if ((SexContent.HasValue && vn.SexContent != SexContent.Value) ||
                        (Otome.HasValue && vn.Otome != Otome.Value) ||
                        (Translated.HasValue && vn.Translated != Translated.Value) ||
                        (Released.HasValue && vn.Released != Released.Value))
                        return false;
                    else
                        return true;
                };
            }
        }
    }
}
