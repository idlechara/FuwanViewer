using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.SampleData.Other;
using FuwanViewer.Presentation.ViewModels;

namespace FuwanViewer.Presentation.ViewModels
{
    public class StubVisualNovelWidgetViewModel : VisualNovelWidgetViewModel
    {
        public StubVisualNovelWidgetViewModel()
            : base(new StubVisualNovel(), null)
        {}
    }
    
}
