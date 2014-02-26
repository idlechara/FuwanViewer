using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.SampleData.Other;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Repository;

namespace FuwanViewer.Presentation.ViewModels
{
    public class StubAllVisualNovelsViewModel : AllVisualNovelsViewModel
    {
        public new StubVisualNovelPreviewViewModel VisualNovelPreview { get; set; }

        public StubAllVisualNovelsViewModel() : base(null, new StubVisualNovelService(), null)
        {
            VisualNovelPreview = new StubVisualNovelPreviewViewModel();       
        }
    }
}
