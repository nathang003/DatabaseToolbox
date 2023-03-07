using Caliburn.Micro;
using ToolboxUI.EventModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.ViewModels
{
	public class SettingsViewModel : Conductor<object>
	{

		private readonly IEventAggregator _eventAggregator;

		public SettingsViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
        }

        private string _selectedUserId;
        public string SelectedUserId
        {
            get
            {
                return _selectedUserId;
            }
            set
            {
                _selectedUserId = value;
            }
        }

        public bool CanEdit
        {
            get
            {
                if (Properties.Settings.Default.UserId.Equals(SelectedUserId) == true
                    && Properties.Settings.Default.Settings_Permissions_CanEditOwnPermissions == true
                    && Properties.Settings.Default.Settings_Permissions_CanView == true)
				{
                    return true;
				}
                else if (Properties.Settings.Default.UserId.Equals(SelectedUserId) == false
                    && Properties.Settings.Default.Settings_Permissions_CanEditSubordinatePermissions == true
                    && Properties.Settings.Default.Settings_Permissions_CanView == true)
				{
                    return true;
				}
                else
				{
                    return false;
				}
            }
        }

        public bool DataArchitecturePage_PermissionEditing_IsEnabled
		{
			get
			{
                if (CanEdit == true
                    && DataArchitecture_CanView_Access == true)
                {
					return true;
				}
                else
				{
					return false;
				}
			}
        }

        public bool SettingsPage_PermissionEditing_IsEnabled
        {
            get
            {
                if (CanEdit == true
                    && Properties.Settings.Default.Settings_CanView == true)
                {
					return true;
				}
                else
				{
					return false;
				}
			}
        }

        public bool TermsAndDefinitionsPage_PermissionEditing_IsEnabled
        {
            get
            {
                if (CanEdit == true
                    && Properties.Settings.Default.TermsAndDefinitions_CanView == true)
                {
					return true;
				}
                else
				{
					return false;
				}
			}
        }

        public bool DataArchitecture_CanView_Access
		{
            get
			{
                return Properties.Settings.Default.DataArchitecture_CanView;
			}
			set
			{
                Properties.Settings.Default.DataArchitecture_CanView = value;

                NotifyOfPropertyChange(() => DataArchitecture_CanView_Access);
                NotifyOfPropertyChange(() => DataArchitecturePage_PermissionEditing_IsEnabled);
                NotifyOfPropertyChange(() => CanNavigateToDataArchitecture);
            }
        }

        public bool DataArchitecture_CanRunDatabaseScrape_Access
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunDatabaseScrape;
            }
            set
            {
                Properties.Settings.Default.DataArchitecture_CanRunDatabaseScrape = value;

                NotifyOfPropertyChange(() => DataArchitecture_CanRunDatabaseScrape_Access);
            }
        }

        public bool DataArchitecture_CanRunSchemaScrape_Access
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunSchemaScrape;
            }
            set
            {
                Properties.Settings.Default.DataArchitecture_CanRunSchemaScrape = value;

                NotifyOfPropertyChange(() => DataArchitecture_CanRunSchemaScrape_Access);
            }
        }

        public bool DataArchitecture_CanRunTableScrape_Access
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunTableScrape;
            }
            set
            {
                Properties.Settings.Default.DataArchitecture_CanRunTableScrape = value;

                NotifyOfPropertyChange(() => DataArchitecture_CanRunTableScrape_Access);
            }
        }

        public bool DataArchitecture_CanRunFieldScrape_Access
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunFieldScrape;
            }
            set
            {
                Properties.Settings.Default.DataArchitecture_CanRunFieldScrape = value;

                NotifyOfPropertyChange(() => DataArchitecture_CanRunFieldScrape_Access);
            }
        }

        public bool DataArchitecture_CanViewDevelopmentServers_Access
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanViewDevelopmentServers;
            }
            set
            {
                Properties.Settings.Default.DataArchitecture_CanViewDevelopmentServers = value;

                NotifyOfPropertyChange(() => DataArchitecture_CanViewDevelopmentServers_Access);
            }
        }

        public bool Settings_CanView_Access
        {
            get
            {
                return Properties.Settings.Default.Settings_CanView;
            }
            set
            {
                Properties.Settings.Default.Settings_CanView = value;

                NotifyOfPropertyChange(() => Settings_CanView_Access);
                NotifyOfPropertyChange(() => SettingsPage_PermissionEditing_IsEnabled);

            }
        }

        public bool Settings_Permissions_CanView_Access
        {
            get
            {
                return Properties.Settings.Default.Settings_Permissions_CanView;
            }
            set
            {
                Properties.Settings.Default.Settings_Permissions_CanView = value;

                NotifyOfPropertyChange(() => Settings_Permissions_CanView_Access);
            }
        }

        public bool Settings_Permissions_CanEditOwnPermissions_Access
        {
            get
            {
                return Properties.Settings.Default.Settings_Permissions_CanEditOwnPermissions;
            }
            set
            {
                Properties.Settings.Default.Settings_Permissions_CanEditOwnPermissions = value;

                NotifyOfPropertyChange(() => Settings_Permissions_CanEditOwnPermissions_Access);
            }
        }

        public bool Settings_Permissions_CanEditSubordinatePermissions_Access
        {
            get
            {
                return Properties.Settings.Default.Settings_Permissions_CanEditSubordinatePermissions;
            }
            set
            {
                Properties.Settings.Default.Settings_Permissions_CanEditSubordinatePermissions = value;

                NotifyOfPropertyChange(() => Settings_Permissions_CanEditSubordinatePermissions_Access);
            }
        }

        public bool TermsAndDefinitions_CanView_Access
        {
            get
            {
                return Properties.Settings.Default.TermsAndDefinitions_CanView;
            }
            set
            {
                Properties.Settings.Default.TermsAndDefinitions_CanView = value;

                NotifyOfPropertyChange(() => TermsAndDefinitions_CanView_Access);
                NotifyOfPropertyChange(() => TermsAndDefinitionsPage_PermissionEditing_IsEnabled);
                NotifyOfPropertyChange(() => CanNavigateToTermsAndDefinitions);
            }
        }

        public bool TermsAndDefinitions_CanAddTerm_Access
        {
            get
            {
                return Properties.Settings.Default.TermsAndDefinitions_CanAddTerm;
            }
            set
            {
                Properties.Settings.Default.TermsAndDefinitions_CanAddTerm = value;

                NotifyOfPropertyChange(() => TermsAndDefinitions_CanAddTerm_Access);
            }
        }

        public bool TermsAndDefinitions_CanEditTerm_Access
        {
            get
            {
                return Properties.Settings.Default.TermsAndDefinitions_CanEditTerm;
            }
            set
            {
                Properties.Settings.Default.TermsAndDefinitions_CanEditTerm = value;

                NotifyOfPropertyChange(() => TermsAndDefinitions_CanEditTerm_Access);
            }
        }

        public bool TermsAndDefinitions_CanRemoveTerm_Access
        {
            get
            {
                return Properties.Settings.Default.TermsAndDefinitions_CanRemoveTerm;
            }
            set
            {
                Properties.Settings.Default.TermsAndDefinitions_CanRemoveTerm = value;

                NotifyOfPropertyChange(() => TermsAndDefinitions_CanRemoveTerm_Access);
            }
        }

        #region Navigation
        public bool CanNavigateToDataArchitecture
		{
            get
			{
                if (Properties.Settings.Default.DataArchitecture_CanView)
                {
					return true;
				}
                else
				{
					return false;
				}
			}
        }

        public bool CanNavigateToTermsAndDefinitions
        {
            get
            {
                if (Properties.Settings.Default.TermsAndDefinitions_CanView)
                {
					return true;
				}
                else
				{
					return false;
				}
			}
        }

        public async Task NavigateToDataArchitecture()
        {
            _eventAggregator.PublishOnUIThread(new NavigateToViewEvent("Database Architecture"));
        }

        public async Task NavigateToTermsAndDefinitions()
        {
            _eventAggregator.PublishOnUIThread(new NavigateToViewEvent("Terms and Definitions"));
        }
        #endregion
    }
}
