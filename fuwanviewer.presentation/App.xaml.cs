using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using FuwanViewer.Model;
using FuwanViewer.Presentation.ViewModels;
using FuwanViewer.Repository;
using FuwanViewer.Repository.Fake;
using FuwanViewer.Services;
using System.Reflection;
using System.Text;
using FuwanViewer.Presentation.Views;

namespace FuwanViewer.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.MainWindow = new MainWindow();

            bool isFakeMode = FuwanViewer.Presentation.Properties.Settings.Default.FakeMode;
            string filePath = GetFilePathForService(isFakeMode);

            // Attempt to restore VisualNovelService from isolated storage,
            // in order to use previous session's cache. (Image and API)
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            VisualNovelService vnService;
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filePath, FileMode.Create, f))
            {
                if (stream.Length > 0)
                {
                    DataContractSerializer dcs = new DataContractSerializer(typeof(VisualNovelService));
                    try
                    {
                         vnService = dcs.ReadObject(stream) as VisualNovelService;
                    }
                    catch (SerializationException)
                    {
                        vnService = VisualNovelServiceFactory.Create(isFakeMode);
                        InitializeServiceProperties(vnService);
                    }
                }
                // if failed to restore service, create a new one with application settings.
                else
                {
                    vnService = VisualNovelServiceFactory.Create(isFakeMode);
                    InitializeServiceProperties(vnService);
                }
            }
            Application.Current.Properties["VisualNovelService"] = vnService;

            // set authentication service
            Application.Current.Properties["AuthenticationService"] = new AuthenticationService();

            // here will e similar attempt to restor UserService ... I believe so at least
            Application.Current.Properties["UserService"] = null;

            // create view model - set as data context - display
            var viewModel = new MainWindowViewModel();
            MainWindow.DataContext = viewModel;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Store the VisualNovelService (overwrite the old one)
            bool isFakeMode = FuwanViewer.Presentation.Properties.Settings.Default.FakeMode;
            string filePath = GetFilePathForService(isFakeMode);
            
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filePath, FileMode.Create, f))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(VisualNovelService));
                dcs.WriteObject(stream, Application.Current.Properties["VisualNovelService"]);
            }

            // TO DO - store the UserService
        }

        #region Function Helpers

        private void InitializeServiceProperties(VisualNovelService vnService)
        {
            vnService.CacheMaximumSize = FuwanViewer.Presentation.Properties.Settings.Default.CacheMaxSize;
            vnService.LimitCacheSize = FuwanViewer.Presentation.Properties.Settings.Default.ConstrainCache;
            vnService.CacheImages = FuwanViewer.Presentation.Properties.Settings.Default.CacheImages;

            if (String.IsNullOrEmpty(FuwanViewer.Presentation.Properties.Settings.Default.CacheDirectory))
                FuwanViewer.Presentation.Properties.Settings.Default.CacheDirectory = GetExeDirectoryPath() + "\\cache";
            vnService.CacheDirectory = FuwanViewer.Presentation.Properties.Settings.Default.CacheDirectory;

            vnService.FuwaUsername = FuwanViewer.Presentation.Properties.Settings.Default.FuwaUsername;
            vnService.FuwaPassword = FuwanViewer.Presentation.Properties.Settings.Default.FuwaPassword;
        }

        private string GetFilePathForService(bool isFakeMode)
        {
            if (isFakeMode)
                return "FakeModeFuwanViewerService";
            else
                return "RealFuwanViewerService";
        }

        private string GetExeDirectoryPath()
        {
            string exeFile = Assembly.GetEntryAssembly().Location;
            return exeFile.Remove(exeFile.LastIndexOf('\\'));
        }

        #endregion // Function Helpers
    }
}
