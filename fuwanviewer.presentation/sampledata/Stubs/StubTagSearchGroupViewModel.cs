using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Presentation.ViewModels.SearchGroups;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubTagSearchGroupViewModel : SearchGroupViewModel
    {
        public ICollection<Pair<string, bool?>> Pairs { get; set; }

        public override Predicate<VisualNovel> SearchPredicate { get { return null; } }

        public StubTagSearchGroupViewModel()
        {
            DisplayName = "Tag search group";

            Pairs = new List<Pair<string, bool?>>()
            {
                new Pair<string, bool?>("Item one", true),
                new Pair<string, bool?>("Item two", null),
                new Pair<string, bool?>("Item three", false),
                new Pair<string, bool?>("Item four", false),
                new Pair<string, bool?>("Item five", true)
            };
        }
    }
}
