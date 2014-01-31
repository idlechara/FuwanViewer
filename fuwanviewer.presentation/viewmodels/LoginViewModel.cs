using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FuwanViewer.Presentation.ViewModels.Abstract;
using FuwanViewer.Services.Interfaces;
using FuwanViewer.Services.Messaging;

namespace FuwanViewer.Presentation.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private IAuthenticationService _authService;
        private Action _loggedInCallback;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsLoggingIn { get; set; }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(Action loggedInCallback)
        {
            _authService = Application.Current.Properties["AuthenticationService"] as IAuthenticationService;
            _loggedInCallback = loggedInCallback;

            Username = FuwanViewer.Presentation.Properties.Settings.Default.FuwaUsername;
            Password = FuwanViewer.Presentation.Properties.Settings.Default.FuwaPassword;
            ErrorMessage = null;

            LoginCommand = new RelayCommand(Login);
        }

        public async void Login(object param)
        {

            var passwordBox = param as System.Windows.Controls.PasswordBox;
            this.Password = passwordBox.Password;

            IsLoggingIn = true;
            OnPropertyChanged("IsLoggingIn");

            UiServices.SetBusyState();
            AuthenticationResponse response = await Task.Run<AuthenticationResponse>(() =>
            {
                return _authService.Authenticate(new AuthenticationRequest(Username, Password));
            });
            UiServices.ClearBusyState();

            // if success => save settings and call callback function
            if (response.success == true)
            {
                FuwanViewer.Presentation.Properties.Settings.Default.FuwaUsername = Username;
                FuwanViewer.Presentation.Properties.Settings.Default.FuwaPassword = Password;
                FuwanViewer.Presentation.Properties.Settings.Default.Save();
                
                if (_loggedInCallback != null)
                    _loggedInCallback();
            }
            // if fail => set message, 
            else
            {
                IsLoggingIn = false;
                switch (response.error)
                {
                    case AuthenticationError.UnableToConnect:
                        ErrorMessage = "Failed to connect to Fuwanovel.org, either Fuwanovel is down or you don't have internet access :(";
                        break;
                    case AuthenticationError.WrongCredentials:
                        ErrorMessage = "Invalid username or password";
                        break;
                    case AuthenticationError.UnknownError:
                        ErrorMessage = "Ups, an error occured. Try login again and if problem keeps happening tell us about it on forums.fuwanovel.org";
                        break;
                    default:
                        break;
                }
                OnPropertyChanged("ErrorMessage");
                OnPropertyChanged("IsError");
                OnPropertyChanged("IsLoggingIn");
            }
        }
    }
}
