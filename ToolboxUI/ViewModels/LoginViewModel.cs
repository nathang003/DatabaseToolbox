using Caliburn.Micro;
using ToolboxUI.EventModels;
using ToolboxUI.Library.Api;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToolboxUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName;
        private string _password;
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                    output = true;

                return output;
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;

                if (_errorMessage?.Length > 0)
                    output = true;

                return output;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        public async Task LogIn()
        {

            Console.WriteLine("Entering ToolboxUI.ViewModels.LoginViewModel.LogIn");

            try
            {

                ErrorMessage = "";

                Console.WriteLine("LoginViewModel.LogIn: _apiHelper.Authenticate attempting to pull results");

                var result = await _apiHelper.Authenticate(UserName, Password);

                Console.WriteLine("LoginViewModel.LogIn: _apiHelper.Authenticate pulled results");

                // Capture more information about the user
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                // TODO: Get user permissions

                Console.WriteLine("LoginViewModel.LogIn: _apiHelper.GetLoggedInUserInfo completed");

                _events.PublishOnUIThread(new LogOnEvent());

            }
            catch (Exception ex)
            {

                Console.WriteLine("LoginViewModel.LogIn: Exception Identified");

                ErrorMessage = ex.Message;

            }
            
        }

        public async Task AttemptLogIn(KeyEventArgs keyArgs)
        {
            if (CanLogIn &&
                    (keyArgs.Key == Key.Enter ||
                    keyArgs.Key == Key.Return))
            {
                await LogIn();
            }
        }
    }
}
