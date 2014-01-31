using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FuwanViewer.Model.VisualNovels;
using FuwanViewer.Presentation;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Services;

namespace FuwanViewer.Presentation.ViewModels
{
    public class ControlPanelViewModel : ViewModelBase
    {
        public bool OpenTabMenuVisible { get; set; }

        public bool IsUpdating { get; set; }
        public ICommand ShowSettingsCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand AddTabButtonCommand { get; set; }
        public ICommand OpenAllNovelsCommand { get; set; }
        public ICommand OpenFeaturedNovelsCommand { get; set; }

        public ControlPanelViewModel(
            Action refreshWorkspaces, 
            Action openSettingsDialog,
            Action addAllNovelsWorkspace,
            Action addFeaturedNovelsWorkspace)
        {
            base.DisplayName = "Control Panel";

            this.RefreshCommand = new RelayCommand(refreshWorkspaces);
            this.ShowSettingsCommand = new RelayCommand(openSettingsDialog);

            this.AddTabButtonCommand = new RelayCommand(ToggleOpenTabMenu);
            this.OpenAllNovelsCommand = new RelayCommand(() =>
                {
                    addAllNovelsWorkspace();
                    ToggleOpenTabMenu();
                });
            this.OpenFeaturedNovelsCommand = new RelayCommand(() =>
                {
                    addFeaturedNovelsWorkspace();
                    ToggleOpenTabMenu();
                });
        }

        void ToggleOpenTabMenu()
        {
            OpenTabMenuVisible = !OpenTabMenuVisible;
            OnPropertyChanged("OpenTabMenuVisible");
        }
    }
}
