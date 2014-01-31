using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Presentation.ViewModels.Abstract;

namespace FuwanViewer.Presentation.SampleData.Stubs
{
    public class StubScreenshotViewModel : ViewModelBase
    {
        public BitmapImage MainArt { get; set; }

        public StubScreenshotViewModel()
        {
            var vn = new StubVisualNovel();

            MainArt = vn.MainArt;
            
        }
    }
}
