using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using FuwanViewer.Model.Users;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Presentation.Views;
using FuwanViewer.Repository;
using FuwanViewer.Repository.Fake;
using FuwanViewer.Services.Interfaces;

namespace FuwanViewer.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields and Properties

        private readonly int _historySize = 4;

        private IAuthenticationService _authService;
        private IVisualNovelService _vnService;
        private HistoryList<VisualNovel> _recentlyViewedNovels;

        public bool Authenticated { get; set; }
        
        public ObservableCollection<WorkspaceViewModel> Workspaces { get; set; }
        public HomeViewModel HomePage { get; set; }
        public LoginViewModel LoginPage { get; set; }

        public ObservableCollection<SidePanelViewModel> SidePanels { get; set; }
        public ObservableCollection<SidePanelViewModel> HiddenPanels { get; set; }

        public ControlPanelViewModel ControlPanel { get; set; }
        public SearchPanelViewModel SearchPanel { get; set; }

        public ICommand ClosingCommand { get; set; }

        #endregion // Fields and Properties

        #region Constructors

        public MainWindowViewModel()
        {
            // initialize fields
            base.DisplayName = "FuwanViewer";
            _recentlyViewedNovels = new HistoryList<VisualNovel>(_historySize);
            _authService = Application.Current.Properties["AuthenticationService"] as IAuthenticationService;
            _vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;

            // initialize commands
            this.ClosingCommand = new RelayCommand(Closing);

            // create home page
            this.HomePage = new HomeViewModel(AddVisualNovelWorkspace, AddAllNovelsWorkspace, AddFeaturedNovelsWorkspace, _recentlyViewedNovels);

            // create control panel
            this.ControlPanel = new ControlPanelViewModel(Refresh, ShowSettings, AddAllNovelsWorkspace, AddFeaturedNovelsWorkspace);

            // create and populate side panels
            this.SidePanels = new ObservableCollection<SidePanelViewModel>();
            SidePanels.CollectionChanged += OnSidePanelsChanged;
            SearchPanel = new SearchPanelViewModel();
            SidePanels.Add(SearchPanel);

            // create hidden panels collection
            this.HiddenPanels = new ObservableCollection<SidePanelViewModel>();
            HiddenPanels.CollectionChanged += OnHiddenPanelsChanged;

            // create workspaces collection
            this.Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.CollectionChanged += OnWorkspacesChanged;
            CollectionViewSource.GetDefaultView(Workspaces).CurrentChanged += SetSearchPanelContext;

            // Authenticate
            var appSettings = FuwanViewer.Presentation.Properties.Settings.Default;

            if (_authService != null)
            {
                Authenticated = _authService.Authenticate(new Services.Messaging.AuthenticationRequest(appSettings.FuwaUsername, appSettings.FuwaPassword)).success;
            }

            // if not authenticated, set Login page
            Action startupAction = GetStartupAction();
            if (Authenticated == false)
            {
                this.LoginPage = new LoginViewModel(() => 
                    {
                        if (startupAction != null)
                            startupAction();
                        Authenticated = true;
                        OnPropertyChanged("Authenticated");
                    });
            }
            // else execute startupAction
            else
            {
                if (startupAction != null)
                    startupAction();
            }
        }

        #endregion // Constructors

        #region Closing workspaces
        /// <summary>
        /// Updates new workspaces items, so that their 'CloseRequest'
        /// event properly removes workspace.
        /// </summary>
        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        /// <summary>
        /// Removes workspace when requested by it
        /// </summary>
        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            if (sender is VisualNovelViewModel)
            {
                _recentlyViewedNovels.Add((sender as VisualNovelViewModel)._novel);
            }
            Workspaces.Remove(workspace);
        }

        #endregion // Closing workspaces

        #region Toggling side panels

        /// <summary>
        /// Updates new side panels, so that their 'ToggleRequest'
        /// event properly hides the panel
        /// </summary>
        void OnSidePanelsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SidePanelViewModel sidePanel in e.NewItems)
                    sidePanel.RequestToggle += HideSidePanel;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SidePanelViewModel sidePanel in e.OldItems)
                    sidePanel.RequestToggle -= HideSidePanel;
        }

        /// <summary>
        /// Updates new side panels, so that their 'ToggleRequest'
        /// event properly restores the panel
        /// </summary>
        void OnHiddenPanelsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (SidePanelViewModel sidePanel in e.NewItems)
                    sidePanel.RequestToggle += ShowSidePanel;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (SidePanelViewModel sidePanel in e.OldItems)
                    sidePanel.RequestToggle -= ShowSidePanel;
        }

        void ShowSidePanel(object sender, EventArgs e)
        {
            SidePanelViewModel sidePanel = sender as SidePanelViewModel;
            HiddenPanels.Remove(sidePanel);
            SidePanels.Add(sidePanel);
        }

        void HideSidePanel(object sender, EventArgs e)
        {
            SidePanelViewModel sidePanel = sender as SidePanelViewModel;
            SidePanels.Remove(sidePanel);
            HiddenPanels.Add(sidePanel);
        }

        #endregion // Toggling side panels

        #region Populating workspaces

        public void AddAllNovelsWorkspace()
        {
            Workspaces.Add(new AllVisualNovelsViewModel(AddVisualNovelWorkspace));
        }

        public void AddAllNovelsAndSetActive()
        {
            var workspace = new AllVisualNovelsViewModel(AddVisualNovelWorkspace);
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        public void AddVisualNovelWorkspace(VisualNovel vn)
        {
            Workspaces.Add(new VisualNovelViewModel(vn));
        }

        public void AddFeaturedNovelsWorkspace()
        {
            Workspaces.Add(new FeaturedVisualNovelsViewModel((o) => AddVisualNovelWorkspace(o as VisualNovel)));
        }

        #endregion // Populating workspaces

        #region Storing and Restoring app state

        private void RestoreLastState()
        {
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            using (IsolatedStorageFileStream stream = f.OpenFile("LastState", System.IO.FileMode.OpenOrCreate))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                try
                {
                    while (stream.Position < stream.Length)
                    {
                        switch ((InformationType) reader.ReadByte())
                        {
                            case InformationType.WorkspacesList:
                                RestoreWorkspacesList(reader);
                                break;
                            case InformationType.HistoryList:
                                RestoreHistoryList(reader);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception) { }
                stream.Close();
            }
        }
        private void SaveLastState()
        {
            IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForAssembly();
            using (IsolatedStorageFileStream stream = f.OpenFile("LastState", System.IO.FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                SaveWorkspaces(writer);
                SaveHistory(writer);
                stream.Close();
            }
        }

        private void SaveWorkspaces(BinaryWriter writer)
        {
            if (Workspaces.Count > 0)
            {
                writer.Write((byte)InformationType.WorkspacesList);

                // number of workspaces
                writer.Write((int)Workspaces.Count);
                // active index
                writer.Write((int)CollectionViewSource.GetDefaultView(this.Workspaces).CurrentPosition);
                // workspaces themselves
                foreach (WorkspaceViewModel workspace in Workspaces)
                {
                    var workType = GetWorkspaceType(workspace);

                    if (workType == WorkspaceType.Unknown)
                        continue;

                    // write workspace type
                    writer.Write((byte)workType);

                    // if single novel then write int32, to mark the novel id
                    if (workType == WorkspaceType.SingleNovel)
                        writer.Write((int)(workspace as VisualNovelViewModel)._novel.Id);
                }
            }
        }
        private void SaveHistory(BinaryWriter writer)
        {
            if (_recentlyViewedNovels.Count > 0)
            {
                writer.Write((byte)InformationType.HistoryList);

                // number of entries
                writer.Write((int)_recentlyViewedNovels.Count);
                // entries themselves
                foreach (VisualNovel vn in _recentlyViewedNovels)
                {
                    writer.Write((int)vn.Id);
                }
            }
        }
        private void RestoreWorkspacesList(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            int index = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                WorkspaceType workType = (WorkspaceType)reader.ReadByte();
                switch (workType)
                {
                    case WorkspaceType.AllNovels:
                        AddAllNovelsWorkspace();
                        break;
                    case WorkspaceType.FeaturedNovels:
                        AddFeaturedNovelsWorkspace();
                        break;
                    case WorkspaceType.SingleNovel:
                        int id = reader.ReadInt32();
                        var vn = _vnService.GetVisualNovel(id);
                        AddVisualNovelWorkspace(vn);
                        break;
                    case WorkspaceType.Unknown:
                    default:
                        break;
                }
            }
            CollectionViewSource.GetDefaultView(this.Workspaces).MoveCurrentToPosition(index);
        }
        private void RestoreHistoryList(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int id = reader.ReadInt32();
                _recentlyViewedNovels.Add(_vnService.GetVisualNovel(id));
            }
        }

        private WorkspaceType GetWorkspaceType(WorkspaceViewModel workspace)
        {
            if (workspace is AllVisualNovelsViewModel)
                return WorkspaceType.AllNovels;
            else if (workspace is FeaturedVisualNovelsViewModel)
                return WorkspaceType.FeaturedNovels;
            else if (workspace is VisualNovelViewModel)
                return WorkspaceType.SingleNovel;
            else
                return WorkspaceType.Unknown;
        }

        private enum WorkspaceType
        {
            AllNovels,
            FeaturedNovels,
            SingleNovel,
            Unknown
        }

        private enum InformationType
        {
            WorkspacesList,
            HistoryList
        }

        #endregion // Storing and Restoring app state

        #region Helper functions

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        private void SetSearchPanelContext(object sender, EventArgs e)
        {
            var workspaces = sender as ICollectionView;
            if (workspaces.CurrentItem is AllVisualNovelsViewModel)
                SearchPanel.Context = (workspaces.CurrentItem as AllVisualNovelsViewModel).AllNovels;
        }

        private Action GetStartupAction()
        {
            switch (FuwanViewer.Presentation.Properties.Settings.Default.OnStartupAction)
            {
                case OnStartupAction.OpenHomeTab:
                    return null;
                case OnStartupAction.OpenAllNovels:
                    return AddAllNovelsAndSetActive;
                case OnStartupAction.RestoreLastState:
                    return RestoreLastState;
                default:
                    return null;
            }
        }

        private void ShowSettings()
        {
            var viewModel = new SettingsDialogViewModel();
            var settingsDialog = new SettingsDialog();
            settingsDialog.DataContext = viewModel;

            settingsDialog.ShowDialog();
        }

        private void Refresh()
        {
            foreach (var workspace in Workspaces)
            {
                workspace.Refresh();
            }
        }

        private void Closing()
        {
            if (FuwanViewer.Presentation.Properties.Settings.Default.OnStartupAction == OnStartupAction.RestoreLastState)
                SaveLastState();
        }

        #endregion // Helper functions
    }
}
