using Caliburn.Micro;
using ToolboxUI.EventModels;
using ToolboxUI.Library.Api;
using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ToolboxUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<MessageEvent>, IHandle<NavigateToViewEvent>, IHandle<QuitEvent>
    {
        private string _firstName;
        private string _lastName;
        private IEventAggregator _events;
        private DataArchitectureViewModel _dataArchitectureVM;
        private SettingsViewModel _settingsVM;
        private TermsAndDefinitionsViewModel _termsAndDefinitionsVM;
        private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, DataArchitectureViewModel dataArchitectureVM, TermsAndDefinitionsViewModel termsAndDefinitionsVM, SettingsViewModel settingsVM, SimpleContainer container)
        {
            _events = events;
            _dataArchitectureVM = dataArchitectureVM;
            _settingsVM = settingsVM;
            _termsAndDefinitionsVM = termsAndDefinitionsVM;
            _container = container;

            _events.Subscribe(this);

            ActivateItem(_container.GetInstance<LoginViewModel>());
            //ActivateItem(_container.GetInstance<DataArchitectureViewModel>());
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string FullName
        {
            get
            {
                return $"{ FirstName } { LastName }";
            }
        }

        public void Load_DataArchitectureView()
        {
            ActivateItem(_dataArchitectureVM);
        }

        public void QuitToolbox()
        {
            foreach (System.Windows.Window window in App.Current.Windows)
            {
                window.Close();
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        private Visibility _sideMenuVisibility = Visibility.Collapsed;
        public Visibility SideMenuVisibility
        {
            get
            {
                return _sideMenuVisibility;
            }
            set
            {
                _sideMenuVisibility = value;
            }
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_dataArchitectureVM);
            SideMenuVisibility = Visibility.Visible;           
        }

        public void Handle(MessageEvent message)
        {
            Status = message.Message;
        }

        public void Handle(NavigateToViewEvent message)
        {
            switch(message.ViewModel)
            {
                case "Terms and Definitions":
                    ActivateItem(_termsAndDefinitionsVM);
                    break;
                case "Settings":
                    ActivateItem(_settingsVM);
                    break;
                case "Database Architecture":
                default:
                    ActivateItem(_dataArchitectureVM);
                    break;
            }
        }

        public void Handle(QuitEvent message)
        {
            QuitToolbox();
        }
    }
}
