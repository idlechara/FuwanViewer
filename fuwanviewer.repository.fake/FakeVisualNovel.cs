using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.VisualNovels;
using System.Windows.Media.Imaging;
using FuwanViewer.Repository.Fake.Proxy;

namespace FuwanViewer.Repository.Fake
{
    internal class FakeVisualNovel : VisualNovel
    {
        private Dictionary<ResourceType, Uri> _uris = new Dictionary<ResourceType,Uri>();
        public Dictionary<ResourceType, Uri> Uris { get { return _uris; } set { _uris = value; } }

        public FakeVisualNovel(string imageFolder)
        {
            foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
	        {
                Uris.Add(
                    resource,
                    new Uri(String.Format("pack://application:,,,/FuwanViewer.Repository.Fake;component/Images/{0}/{0}-{1}.jpg", imageFolder, resource.ToString())));
	        }
            
        }
    }        
    
}
