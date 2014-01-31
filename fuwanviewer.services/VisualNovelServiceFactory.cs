using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Repository;
using FuwanViewer.Repository.Fake;

namespace FuwanViewer.Services
{
    /// <summary>
    /// Provides static method to aid creation of VisualNovelService class.
    /// </summary>
    public static class VisualNovelServiceFactory
    {
        /// <summary>
        /// Initializes new instance of VisualNovelService either for Fake or Real mode.
        /// </summary>
        /// <param name="isFakeMode">True if FakeRepository should be used, False if Fuwanovel.org should be used.</param>
        public static VisualNovelService Create(bool isFakeMode)
        {
            IVisualNovelRepository repository;
            if (isFakeMode == true)
            {
                repository = new FakeFuwaVNRepository();
            }
            else
            {
                repository = new FuwaVNRepository();
            }
            return new VisualNovelService(repository);
        }
    }
}
