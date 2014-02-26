using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.SampleData.Other;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.ViewModels
{
    class StubFeaturedVisualNovelsViewModel : FeaturedVisualNovelsViewModel
    {
        public StubFeaturedVisualNovelsViewModel()
            : base(null, new StubVisualNovelService())
        { }
    }
}
