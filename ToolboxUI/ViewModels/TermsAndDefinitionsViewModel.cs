using Caliburn.Micro;
using Dapper;
using ToolboxUI.EventModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.ViewModels
{
    public class TermsAndDefinitionsViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        public TermsAndDefinitionsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public bool CanNavigateToDataArchitectureView
        {
            get
            {
                if (Properties.Settings.Default.DataArchitecture_CanView)
                    return true;
                else
                    return false;
            }
        }

        public bool CanNavigateToSettingsView
        {
            get
            {
                if (Properties.Settings.Default.TermsAndDefinitions_CanView)
                    return true;
                else
                    return false;
            }
        }

        public async Task NavigateToDataArchitectureView()
        {
            _eventAggregator.PublishOnUIThread(new NavigateToViewEvent("Database Architecture"));
        }

        public async Task NavigateToSettingsView()
        {
            _eventAggregator.PublishOnUIThread(new NavigateToViewEvent("Settings"));
        }
    }
}
