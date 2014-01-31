using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Services.Interfaces;

namespace FuwanViewer.Presentation.ViewModels.SettingGroups
{
    public class GeneralSettingGroupViewModel : SettingsGroupViewModel
    {
        private static readonly Dictionary<OnStartupAction, string> _messages = new Dictionary<OnStartupAction, string>()
        {
            {OnStartupAction.OpenAllNovels, "Open 'All novels' tab"},
            {OnStartupAction.OpenHomeTab, "Open home tab"},
            {OnStartupAction.RestoreLastState, "Restore last view"}
        };
        
        private IVisualNovelService _vnService = Application.Current.Properties["VisualNovelService"] as IVisualNovelService;

        public List<OnStartupEntry> OnStartupOptions { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        

        public GeneralSettingGroupViewModel()
        {
            base.DisplayName = "General";

            Username = FuwanViewer.Presentation.Properties.Settings.Default.FuwaUsername;
            Password = FuwanViewer.Presentation.Properties.Settings.Default.FuwaPassword;

            OnStartupOptions = CreateOnStartupEntries();
        }

        public override void SaveSettings()
        {
            _vnService.FuwaUsername = Username;
            FuwanViewer.Presentation.Properties.Settings.Default.FuwaUsername = Username;

            _vnService.FuwaPassword = Password;
            FuwanViewer.Presentation.Properties.Settings.Default.FuwaPassword = Password;

            FuwanViewer.Presentation.Properties.Settings.Default.OnStartupAction = OnStartupOptions.Single((entry) => entry.Selected == true).Value;

            FuwanViewer.Presentation.Properties.Settings.Default.Save();

            base.SaveSettings();
        }

        private List<OnStartupEntry> CreateOnStartupEntries()
        {
            var result = new List<OnStartupEntry>();
            var actionFromSettings = FuwanViewer.Presentation.Properties.Settings.Default.OnStartupAction;
            foreach (OnStartupAction action in _messages.Keys)
            {
                result.Add(new OnStartupEntry(_messages[action], action == actionFromSettings, action));
            }
            return result;
        }

        public class OnStartupEntry
        {
            public string Message { get; set;}
            public bool Selected { get; set; }
            public OnStartupAction Value { get; set; }

            public OnStartupEntry(string message, bool selected, OnStartupAction value)
            {
                this.Message = message;
                this.Selected = selected;
                this.Value = value;
            }
        }
    }
}
