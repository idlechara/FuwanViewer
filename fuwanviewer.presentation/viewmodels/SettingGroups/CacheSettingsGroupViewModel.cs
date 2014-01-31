using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Services.Interfaces;

namespace FuwanViewer.Presentation.ViewModels.SettingGroups
{
    public class CacheSettingsGroupViewModel : SettingsGroupViewModel
    {
        #region Fields and Properties

        IVisualNovelService _vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;
        SizeUnit _sizeUnit;
        long _maxSize;
        string _cacheDirectory;

        public bool EnableCaching { get; set; }
        public bool LimitSize { get; set; }
        public string CacheDirectory
        {
            get { return _cacheDirectory; }
            set
            {
                _cacheDirectory = value;
                OnPropertyChanged();
                OnPropertyChanged("MissingDirectoryMessageVisibility");
                OnPropertyChanged("InvalidDirectoryMessageVisibility");
            }
        }

        public SizeUnit SizeUnit
        {
            get { return _sizeUnit; }
            set
            {
                _sizeUnit = value;
                OnPropertyChanged("MaxSize");
            }
        }
        /// <summary>
        /// Represents maximum cache size in SizeUnits. (KB or MB)
        /// </summary>
        public long MaxSize
        {
            get 
            {
                return _maxSize / (int)_sizeUnit; 
            }
            set 
            { 
                _maxSize = value * (int) _sizeUnit;
            }
        }
        
        public Visibility MissingDirectoryMessageVisibility
        {
            get 
            { 
                return EnableCaching == true && IsValidDirectory(CacheDirectory) && Directory.Exists(CacheDirectory) == false ? 
                    Visibility.Visible : 
                    Visibility.Collapsed; 
            }
        }
        public Visibility InvalidDirectoryMessageVisibility
        {
            get 
            { 
                return EnableCaching == true && IsValidDirectory(CacheDirectory) == false ? 
                    Visibility.Visible : 
                    Visibility.Collapsed; 
            }
        }

        public ICommand FolderBrowseCommand { get; set; }

        #endregion // Fields and Properties

        #region Constructor

        public CacheSettingsGroupViewModel()
        {
            base.DisplayName = "Caching";
            _sizeUnit = SizeUnit.MB;

            EnableCaching = FuwanViewer.Presentation.Properties.Settings.Default.CacheImages;
            LimitSize = FuwanViewer.Presentation.Properties.Settings.Default.ConstrainCache;
            MaxSize = FuwanViewer.Presentation.Properties.Settings.Default.CacheMaxSize / (int) SizeUnit;
            CacheDirectory = FuwanViewer.Presentation.Properties.Settings.Default.CacheDirectory;

            FolderBrowseCommand = new RelayCommand(FolderBrowse);
        }

        #endregion // Constructor

        #region Command Actions

        void FolderBrowse(object param)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.RootFolder = System.Environment.SpecialFolder.MyComputer;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CacheDirectory = dialog.SelectedPath;
            }
        }

        #endregion // Command Actions

        #region Public Interface

        public override void SaveSettings()
        {
            _vnService.CacheImages = EnableCaching;
            Properties.Settings.Default.CacheImages = EnableCaching;

            _vnService.LimitCacheSize = LimitSize;
            Properties.Settings.Default.ConstrainCache = LimitSize;

            _vnService.CacheMaximumSize = MaxSize * (int) SizeUnit;
            Properties.Settings.Default.CacheMaxSize = MaxSize * (int)SizeUnit;

            if (IsValidDirectory(CacheDirectory) == true && EnableCaching == true)
            {
                if (Directory.Exists(CacheDirectory) == false)
                {
                    CreateDirectoryStructure(CacheDirectory);
                }
                _vnService.CacheDirectory = CacheDirectory;
                Properties.Settings.Default.CacheDirectory = CacheDirectory;
            }

            Properties.Settings.Default.Save();

            base.SaveSettings();
        }

        #endregion // Public Interface

        #region Function Helpers

        bool IsValidDirectory(string path)
        {
            if (path == String.Empty || path == null)
                return false;

            // extract components
            string[] components = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string driveName = components[0] + Path.DirectorySeparatorChar;

            // check if drive exists & is either Fixed or Removable type
            DriveInfo[] drives = DriveInfo.GetDrives();
            DriveInfo pathDriveInfo = drives.SingleOrDefault((di) => di.Name == driveName);
            if (pathDriveInfo == null)
                return false;
            else if (pathDriveInfo.DriveType != DriveType.Fixed &&
                     pathDriveInfo.DriveType != DriveType.Removable)
                return false;

            // check for forbidden characters in component names (except drive name);
            var forbiddenChars = Path.GetInvalidFileNameChars();
            for (int i = 1; i < components.Length; i++)
            {
                string item = components[i];
                for (int j = 0; j < item.Length; j++)
                {
                    if (forbiddenChars.Contains(item[j]))
                        return false;
                }
            }

            // check if path ends with space or dot
            if (path.Last() == '.' || path.Last() == ' ')
                return false;

            // otherwise everything is fine
            return true;
        }

        void CreateDirectoryStructure(string path)
        {
            string[] components = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            StringBuilder sb = new StringBuilder(path.Length);
            sb.AppendFormat("{0}\\", components[0]);

            for (int i = 1; i < components.Length; i++)
            {
                sb.AppendFormat("{0}\\", components[i]);

                if (Directory.Exists(sb.ToString()) == false)
                    Directory.CreateDirectory(sb.ToString());
            }
        }

        #endregion // Function Helpers
    }

    public enum SizeUnit
    {
        // value corresponds to number of Bytes
        KB = 1024,
        MB = 1048576
    }
}
